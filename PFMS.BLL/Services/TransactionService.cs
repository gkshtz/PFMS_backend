using AutoMapper;
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
        private readonly ITotalTransactionAmountRespository _totalTransactionAmountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly IBudgetsRepository _budgetsRepository;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper, ITotalTransactionAmountRespository totalTransactionAmountRespository, IBudgetsRepository budgetsRepository, IEmailService emailService, IUserRepository userRepository)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _totalTransactionAmountRepository = totalTransactionAmountRespository;
            _budgetsRepository = budgetsRepository;
            _emailService = emailService;
            _userRepository = userRepository;
        }
        public async Task<List<TransactionBo>> GetAllTransactionsAsync(Guid userId, Filter? filter, Sort? sort, Pagination pagination)
        {
            List<TransactionDto> transactionsDto = await _transactionRepository.GetAllTransactionsAsync(userId, filter, sort, pagination);
            return _mapper.Map<List<TransactionBo>>(transactionsDto);
        }
        public async Task<Guid> AddTransaction(TransactionBo transactionBo, Guid userId)
        {
            var totalTransactionAmountDto = await _transactionRepository.GetTotalTransactionAmountByUserId(userId);

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
                await _totalTransactionAmountRepository.AddTotalMonthlyAmount(totalMonthlyAmountDto);

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

            var lastTransactionDate = (await _transactionRepository.GetTransactionWithLatestDate(totalTransactionAmountBo.TotalTransactionAmountId)).TransactionDate;

            totalTransactionAmountBo.LastTransactionDate = lastTransactionDate;

            await _totalTransactionAmountRepository.UpdateTotalTransactionAmount(_mapper.Map<TotalTransactionAmountDto>(totalTransactionAmountBo));

            transactionBo.TotalTransactionAmountId = totalTransactionAmountBo.TotalTransactionAmountId;
            transactionBo.TransactionId = Guid.NewGuid();

            var transactionDto = _mapper.Map<TransactionDto>(transactionBo);
            await _transactionRepository.AddTransaction(transactionDto);

            var budgetDto = await _budgetsRepository.GetBudgetByUserId(userId, transactionBo.TransactionDate.Month, transactionBo.TransactionDate.Year);
            if(budgetDto != null)
            {
                var budgetBo = _mapper.Map<BudgetBo>(budgetDto);
                if(totalTransactionAmountBo.TotalExpence > budgetBo.BudgetAmount)
                {
                    var userDto = await _userRepository.GetUserById(userId);
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
            var transactionDto = await _transactionRepository.GetByTransactionId(transactionId, userId);
            if (transactionDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.TransactionNotFound);
            }

            return _mapper.Map<TransactionBo>(transactionDto);
        }

        public async Task UpdateTransaction(TransactionBo transactionBoNew, Guid userId, Guid transactionId)
        {
            var totalTransactionAmountDto = await _transactionRepository.GetTotalTransactionAmountByUserId(userId);
            var transactionDto = await _transactionRepository.GetByTransactionId(transactionId, userId);

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

            await _totalTransactionAmountRepository.UpdateTotalTransactionAmount(_mapper.Map<TotalTransactionAmountDto>(totalTransactionAmountBo));

            transactionBoNew.TotalTransactionAmountId = transactionBoOld.TotalTransactionAmountId;
            transactionBoNew.TransactionId = transactionBoOld.TransactionId;

            transactionDto = _mapper.Map<TransactionDto>(transactionBoNew);

            await _transactionRepository.UpdateTransaction(transactionDto, transactionId, totalTransactionAmountBo.TotalTransactionAmountId);
        }

        public async Task DeleteTransaction(Guid transactionId, Guid userId)
        {
            var totalTransactionAmountDto = await _transactionRepository.GetTotalTransactionAmountByUserId(userId);
            var totalTransactionAmountBo = _mapper.Map<TotalTransactionAmountBo>(totalTransactionAmountDto);

            var transactionDto = await _transactionRepository.GetByTransactionId(transactionId, userId);
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

            await _transactionRepository.DeleteTransaction(transactionId, totalTransactionAmountBo.TotalTransactionAmountId);

            transactionDto = await _transactionRepository.GetTransactionWithLatestDate(totalTransactionAmountBo.TotalTransactionAmountId);

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

            await _totalTransactionAmountRepository.UpdateTotalTransactionAmount(totalTransactionAmountDto);
        }

        public async Task<TotalTransactionAmountBo> GetTotalTransactionAmountAsync(Guid userId)
        {
            var totalTransactionAmountDto = await _transactionRepository.GetTotalTransactionAmountByUserId(userId);
            return _mapper.Map<TotalTransactionAmountBo>(totalTransactionAmountDto);
        }
    }
}
