using AutoMapper;
using Microsoft.AspNetCore.Http;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.DAL.DTOs;
using PFMS.DAL.Interfaces;
using PFMS.Utils.Constants;
using PFMS.Utils.CustomExceptions;
using PFMS.Utils.Enums;
using PFMS.Utils.RequestData;

namespace PFMS.BLL.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        public TransactionService(IMapper mapper, IEmailService emailService, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<TransactionBo>> GetAllTransactionsAsync(Guid userId, Filter? filter, Sort? sort, Pagination pagination)
        {
            List<TransactionDto> transactionsDto = await _unitOfWork.TransactionsRepository.GetAllTransactionsAsync(userId, filter, sort, pagination);
            return _mapper.Map<List<TransactionBo>>(transactionsDto);
        }
        public async Task<Guid> AddTransaction(TransactionBo transactionBo, Guid userId, IFormFile? file, string rootPath)
        {
            var totalTransactionAmountDto = await _unitOfWork.TransactionsRepository.GetTotalTransactionAmountByUserId(userId);

            var totalTransactionAmountBo = _mapper.Map<TotalTransactionAmountBo>(totalTransactionAmountDto);

            int currentMonth = DateTime.UtcNow.Month;
            int currentYear = DateTime.UtcNow.Year;

            if(transactionBo.TransactionDate.Month!=currentMonth || transactionBo.TransactionDate.Year!=currentYear)
            {
                throw new BadRequestException(ErrorMessages.TransactionCanOnlyBeSetOfPresentMonth);
            }

            if (totalTransactionAmountBo.LastTransactionDate.Month < currentMonth || totalTransactionAmountBo.LastTransactionDate.Year < currentYear)
            {
                var totalMonthlyAmountBo = new TotalMonthlyAmountBo()
                {
                    TotalMonthlyAmountId = Guid.NewGuid(),
                    TotalExpenceOfMonth = totalTransactionAmountBo.TotalExpence,
                    TotalIncomeOfMonth = totalTransactionAmountBo.TotalIncome,
                    Month = totalTransactionAmountBo.LastTransactionDate.Month,
                    Year = totalTransactionAmountBo.LastTransactionDate.Year,
                    TotalTransactionAmountId = totalTransactionAmountBo.TotalTransactionAmountId
                };

                var totalMonthlyAmountDto = _mapper.Map<TotalMonthlyAmountDto>(totalMonthlyAmountBo);
                await _unitOfWork.TotalTransactionAmountsRespository.AddTotalMonthlyAmount(totalMonthlyAmountDto);

                totalTransactionAmountBo.TotalExpence = 0;
                totalTransactionAmountBo.TotalIncome = 0;
            }

            if (transactionBo.TransactionType == Utils.Enums.TransactionType.Income)
            {
                totalTransactionAmountBo.TotalIncome += transactionBo.TransactionAmount;
            }
            else if (transactionBo.TransactionType == Utils.Enums.TransactionType.Expense)
            {
                totalTransactionAmountBo.TotalExpence += transactionBo.TransactionAmount;
            }

            var lastTransactionDate = (await _unitOfWork.TransactionsRepository.GetTransactionWithLatestDate(totalTransactionAmountBo.TotalTransactionAmountId)).TransactionDate;

            totalTransactionAmountBo.LastTransactionDate = lastTransactionDate;

            await _unitOfWork.TotalTransactionAmountsRespository.UpdateTotalTransactionAmount(_mapper.Map<TotalTransactionAmountDto>(totalTransactionAmountBo));

            transactionBo.TotalTransactionAmountId = totalTransactionAmountBo.TotalTransactionAmountId;
            transactionBo.TransactionId = Guid.NewGuid();

            var transactionDto = _mapper.Map<TransactionDto>(transactionBo);
            await _unitOfWork.TransactionsRepository.AddTransaction(transactionDto);

            // Add Screenshot if available
            string filePath="";
            if(file != null)
            {
                filePath = Path.Combine(rootPath, "Screenshots", file.FileName);

                var transactionScreenshotBo = new TransactionScreenshotBo()
                {
                    ScreenshotId = Guid.NewGuid(),
                    FileName = Path.GetFileNameWithoutExtension(file.FileName),
                    FileSizeInBytes = file.Length,
                    FileExtension = Path.GetExtension(file.FileName),
                    FilePath = filePath,
                    TransactionId = transactionBo.TransactionId
                };

                var transactionScreenshotDto = _mapper.Map<TransactionScreenshotDto>(transactionScreenshotBo);
                await _unitOfWork.ScreenshotsRepository.AddScreenshot(transactionScreenshotDto);
            }

            await _unitOfWork.SaveDatabaseChangesAsync();

            if(file!=null)
            {
                using var fileStream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(fileStream);
            }

            var budgetDto = await _unitOfWork.BudgetsRepository.GetBudgetByUserId(userId, transactionBo.TransactionDate.Month, transactionBo.TransactionDate.Year);
            if(budgetDto != null)
            {
                var budgetBo = _mapper.Map<BudgetBo>(budgetDto);
                if(totalTransactionAmountBo.TotalExpence > budgetBo.BudgetAmount)
                {
                    var userDto = await _unitOfWork.UsersRepository.GetUserById(userId);
                    var userBo = _mapper.Map<UserBo>(userDto);

                    var subject = ApplicationConstsants.BudgetExceededMailSubject;
                    decimal exceededAmount = totalTransactionAmountBo.TotalExpence - budgetBo.BudgetAmount;

                    var body = ApplicationConstsants.GenerateBudgetExceededMailBody(userBo.FirstName, (Months)(transactionBo.TransactionDate.Month-1), transactionBo.TransactionDate.Year, exceededAmount);

                    await _emailService.SendEmail(userBo.Email, subject, body);
                }
            }
            
            return transactionBo.TransactionId;
        }

        public async Task<TransactionBo> GetByTransactionId(Guid transactionId, Guid userId)
        {
            var transactionDto = await _unitOfWork.TransactionsRepository.GetByTransactionId(transactionId, userId);
            if (transactionDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.TransactionNotFound);
            }

            return _mapper.Map<TransactionBo>(transactionDto);
        }

        public async Task UpdateTransaction(TransactionBo transactionBoNew, Guid userId, Guid transactionId, string rootPath)
        {
            var totalTransactionAmountDto = await _unitOfWork.TransactionsRepository.GetTotalTransactionAmountByUserId(userId);
            var transactionDto = await _unitOfWork.TransactionsRepository.GetByTransactionId(transactionId, userId);

            if(transactionDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.TransactionNotFound);
            }

            var transactionBoOld = _mapper.Map<TransactionBo>(transactionDto);

            decimal changeInAmount = transactionBoNew.TransactionAmount - transactionBoOld.TransactionAmount;

            var totalTransactionAmountBo = _mapper.Map<TotalTransactionAmountBo>(totalTransactionAmountDto);

            if(transactionBoNew.TransactionType == Utils.Enums.TransactionType.Expense)
            {
                if(transactionBoOld.TransactionType == Utils.Enums.TransactionType.Expense)
                {
                    totalTransactionAmountBo.TotalExpence += changeInAmount;
                }
                else
                {
                    totalTransactionAmountBo.TotalIncome -= transactionBoOld.TransactionAmount;
                    totalTransactionAmountBo.TotalExpence += transactionBoNew.TransactionAmount;
                }
            }
            else if(transactionBoNew.TransactionType == Utils.Enums.TransactionType.Income)
            {
                if(transactionBoOld.TransactionType == Utils.Enums.TransactionType.Income)
                {
                    totalTransactionAmountBo.TotalIncome += changeInAmount;
                }
                else if(transactionBoOld.TransactionType == Utils.Enums.TransactionType.Expense)
                {
                    totalTransactionAmountBo.TotalExpence -= transactionBoOld.TransactionAmount;
                    totalTransactionAmountBo.TotalIncome += transactionBoNew.TransactionAmount;
                }
            }
            totalTransactionAmountBo.LastTransactionDate = transactionBoNew.TransactionDate;

            await _unitOfWork.TotalTransactionAmountsRespository.UpdateTotalTransactionAmount(_mapper.Map<TotalTransactionAmountDto>(totalTransactionAmountBo));

            transactionBoNew.TotalTransactionAmountId = transactionBoOld.TotalTransactionAmountId;
            transactionBoNew.TransactionId = transactionBoOld.TransactionId;

            transactionDto = _mapper.Map<TransactionDto>(transactionBoNew);

            await _unitOfWork.TransactionsRepository.UpdateTransaction(transactionDto, transactionId, totalTransactionAmountBo.TotalTransactionAmountId);

            if(transactionBoNew.File!=null)
            {
                var newFilePath = Path.Combine(rootPath, "Screenshots", transactionBoNew.File.FileName);

                var screenshotDto = await _unitOfWork.ScreenshotsRepository.GetScreenshotByTransactionId(transactionId);
                if(screenshotDto!=null)
                {
                    var screenshotBo = _mapper.Map<TransactionScreenshotBo>(screenshotDto);

                    File.Delete(screenshotBo.FilePath);

                    screenshotBo.FileName = Path.GetFileNameWithoutExtension(transactionBoNew.File.FileName);
                    screenshotBo.FileSizeInBytes = transactionBoNew.File.Length;
                    screenshotBo.FileExtension = Path.GetExtension(transactionBoNew.File.FileName);
                    screenshotBo.FilePath = newFilePath;

                    screenshotDto = _mapper.Map<TransactionScreenshotDto>(screenshotBo);
                    await _unitOfWork.ScreenshotsRepository.UpdateScreenshot(screenshotDto);
                }
                else
                {
                    var screenshotBo = new TransactionScreenshotBo()
                    {
                        ScreenshotId = Guid.NewGuid(),
                        FileName = Path.GetFileNameWithoutExtension(transactionBoNew.File.FileName),
                        FileExtension = Path.GetExtension(transactionBoNew.File.FileName),
                        FileSizeInBytes = transactionBoNew.File.Length,
                        FilePath = newFilePath,
                        TransactionId = transactionId
                    };

                    screenshotDto = _mapper.Map<TransactionScreenshotDto>(screenshotBo);
                    await _unitOfWork.ScreenshotsRepository.AddScreenshot(screenshotDto);
                }

                FileStream fileStream = new FileStream(newFilePath, FileMode.Create);
                await transactionBoNew.File.CopyToAsync(fileStream);
            }

            await _unitOfWork.SaveDatabaseChangesAsync();
        }

        public async Task DeleteTransaction(Guid transactionId, Guid userId)
        {
            var totalTransactionAmountDto = await _unitOfWork.TransactionsRepository.GetTotalTransactionAmountByUserId(userId);
            var totalTransactionAmountBo = _mapper.Map<TotalTransactionAmountBo>(totalTransactionAmountDto);

            var transactionDto = await _unitOfWork.TransactionsRepository.GetByTransactionId(transactionId, userId);
            if(transactionDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.TransactionNotFound);
            }

            var transactionBo = _mapper.Map<TransactionBo>(transactionDto);

            if(transactionBo.TransactionType == Utils.Enums.TransactionType.Expense)
            {
                totalTransactionAmountBo.TotalExpence -= transactionBo.TransactionAmount;
            }
            else if(transactionBo.TransactionType == Utils.Enums.TransactionType.Income)
            {
                totalTransactionAmountBo.TotalIncome -= transactionBo.TransactionAmount;
            }

            await _unitOfWork.TransactionsRepository.DeleteTransaction(transactionId, totalTransactionAmountBo.TotalTransactionAmountId);

            transactionDto = await _unitOfWork.TransactionsRepository.GetTransactionWithLatestDate(totalTransactionAmountBo.TotalTransactionAmountId);

            if(transactionDto == null)
            {
                totalTransactionAmountBo.LastTransactionDate = DateTime.UtcNow;
            }
            else
            {
                transactionBo = _mapper.Map<TransactionBo>(transactionDto);
                totalTransactionAmountBo.LastTransactionDate = transactionBo.TransactionDate;
            }

            totalTransactionAmountDto = _mapper.Map<TotalTransactionAmountDto>(totalTransactionAmountBo);

            await _unitOfWork.TotalTransactionAmountsRespository.UpdateTotalTransactionAmount(totalTransactionAmountDto);

            await _unitOfWork.SaveDatabaseChangesAsync();
        }

        public async Task<TotalTransactionAmountBo> GetTotalTransactionAmountAsync(Guid userId)
        {
            var totalTransactionAmountDto = await _unitOfWork.TransactionsRepository.GetTotalTransactionAmountByUserId(userId);
            return _mapper.Map<TotalTransactionAmountBo>(totalTransactionAmountDto);
        }

        public async Task<TotalMonthlyAmountBo> GetPreviousMonthSummary(int month, int year, Guid userId)
        {
            // check to ensure the month and year are valid
            if(month < 1 || month > 12 || year < 2000 || year > 3000)
            {
                throw new BadRequestException(ErrorMessages.InvalidMonthOrYear);
            }

            //fetch totalTransactionAmountId for fetching totalMonthlyAmountDto
            var totalTransactionAmountId = (await _unitOfWork.TransactionsRepository.GetTotalTransactionAmountByUserId(userId))?.TotalTransactionAmountId;
            if(totalTransactionAmountId == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            var totalMonthlyAmountDto = await _unitOfWork.TotalTransactionAmountsRespository.GetTotalMonthlyAmountOfParticularMonthAndYear((Guid)totalTransactionAmountId, month, year);

            TotalMonthlyAmountBo totalMonthlyAmountBo;
            if(totalMonthlyAmountDto == null)
            {
                totalMonthlyAmountBo = new TotalMonthlyAmountBo()
                {
                    TotalExpenceOfMonth = 0,
                    TotalIncomeOfMonth = 0,
                    Month = month,
                    Year = year
                };
            }
            else
            {
                totalMonthlyAmountBo = _mapper.Map<TotalMonthlyAmountBo>(totalMonthlyAmountDto);
            }

            //Fetch Budget of that particular month, if present.
            var budgetAmount = (await _unitOfWork.BudgetsRepository.GetBudgetByUserId(userId, month, year))?.BudgetAmount;
            totalMonthlyAmountBo.BudgetAmount = budgetAmount;

            return totalMonthlyAmountBo;
        }
    }
}
