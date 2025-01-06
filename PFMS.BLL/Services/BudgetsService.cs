using AutoMapper;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.DAL.DTOs;
using PFMS.DAL.Interfaces;
using PFMS.DAL.Repositories;
using PFMS.Utils.Constants;
using PFMS.Utils.CustomExceptions;
using PFMS.Utils.Enums;
namespace PFMS.BLL.Services
{
    public class BudgetsService: IBudgetsService
    {
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        public BudgetsService(IMapper mapper, IEmailService emailService, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }
        public async Task AddNewBudget(BudgetBo budgetBo, Guid userId)
        {
            var userDto = await _unitOfWork.UsersRepository.GetUserById(userId);
            if(userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            var userBo = _mapper.Map<UserBo>(userDto);

            // check if the budget is being set for a past month
            if(budgetBo.Year < DateTime.UtcNow.Year || (budgetBo.Year == DateTime.UtcNow.Year && budgetBo.Month < DateTime.UtcNow.Month))
            {
                throw new BadRequestException(ErrorMessages.BudgetCannotBeSetForPast);
            }

            budgetBo.BudgetId = Guid.NewGuid();
            budgetBo.UserId = userId;

            var budgetDto = _mapper.Map<BudgetDto>(budgetBo);

            await _unitOfWork.BudgetsRepository.AddBudget(budgetDto);

            await _unitOfWork.SaveDatabaseChangesAsync();

            await SendMailForBudgetSet(userBo, budgetBo);
        }

        public async Task<BudgetBo> GetBudget(Guid userId, int month, int year)
        {
            if(month < 0 || month>12 || (year!=0 && (year<2000 || year>3000)))
            {
                throw new BadRequestException(ErrorMessages.InvalidMonthOrYear);
            }

            month = month == 0 ? DateTime.UtcNow.Month : month;
            year = year == 0 ? DateTime.UtcNow.Year : year;

            var budgetDto = await _unitOfWork.BudgetsRepository.GetBudgetByUserId(userId, month, year);
            if (budgetDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.BudgetNotFound);
            }
            var budgetBo = _mapper.Map<BudgetBo>(budgetDto);

            var totalTransactionAmountDto = await _unitOfWork.TransactionsRepository.GetTotalTransactionAmountByUserId(userId);
            var totalTransactionAmountBo = _mapper.Map<TotalTransactionAmountBo>(totalTransactionAmountDto);

            decimal totalExpence;
            if(month == DateTime.UtcNow.Month && year == DateTime.UtcNow.Year)
            {
                totalExpence = totalTransactionAmountBo.TotalExpence;
            }
            else
            {
                var totalMonthlyAmountDto = await _unitOfWork.TotalTransactionAmountsRespository.GetTotalMonthlyAmountOfParticularMonthAndYear(totalTransactionAmountBo.TotalTransactionAmountId, month, year);
                var totalMonthlyAmountBo = _mapper.Map<TotalMonthlyAmountBo>(totalMonthlyAmountDto);
                totalExpence = totalMonthlyAmountBo?.TotalExpenceOfMonth ?? 0;
            }

            budgetBo.SpentPercentage = (totalExpence / budgetBo.BudgetAmount) * 100;

            return budgetBo;
        }

        public async Task UpdateBudget(BudgetBo budgetBo, Guid userId, Guid budgetId)
        {
            // check to ensure that budget with this budgetId exists.
            var budgetDto = await _unitOfWork.BudgetsRepository.GetBudgetById(budgetId);
            if(budgetDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.BudgetNotFound);
            }

            //check to ensure that the budget belongs to this user
            var budgetBoOld = _mapper.Map<BudgetBo>(budgetDto);
            if(budgetBoOld.UserId != userId)
            {
                throw new BadRequestException(ErrorMessages.BudgetDoesNotBelongToThisUser);
            }

            //update the budget
            budgetBo.BudgetId = budgetId;
            budgetBo.UserId = userId;
            budgetDto = _mapper.Map<BudgetDto>(budgetBo);

            await _unitOfWork.BudgetsRepository.UpdateBudget(budgetDto);

            //send the email that budget is updated
            var userDto = await _unitOfWork.UsersRepository.GetUserById(userId);
            var userBo = _mapper.Map<UserBo>(userDto);

            await _unitOfWork.SaveDatabaseChangesAsync();

            var subject = ApplicationConstsants.BudgetUpdateMailSubject;
            var body = ApplicationConstsants.GenerateBudgetUpdateEmailSubject(userBo.FirstName, (Months)(budgetBo.Month-1), budgetBo.Year);

            await _emailService.SendEmail(userBo.Email, subject, body);
        }

        public async Task DeleteBudget(Guid budgetId, Guid userId)
        {
            var userDto = await _unitOfWork.UsersRepository.GetUserById(userId);
            if(userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            var budgetDto = await _unitOfWork.BudgetsRepository.GetBudgetById(budgetId);
            if(budgetDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.BudgetNotFound);
            }

            var budgetBo = _mapper.Map<BudgetBo>(budgetDto);
           
            //check to ensure budget belongs to this user
            if(budgetBo.UserId != userId)
            {
                throw new BadRequestException(ErrorMessages.BudgetDoesNotBelongToThisUser);
            }

            await _unitOfWork.BudgetsRepository.DeleteBudget(budgetId);

            await _unitOfWork.SaveDatabaseChangesAsync();
        }

        private async Task SendMailForBudgetSet(UserBo userBo, BudgetBo budgetBo)
        {
            decimal spentPercentage;
            //check if the budget is of future month
            if(budgetBo.Year > DateTime.UtcNow.Year || (budgetBo.Year == DateTime.UtcNow.Year && budgetBo.Month > DateTime.UtcNow.Month))
            {
                spentPercentage = 0;
            }
            else
            {
                var totalTransactionAmountDto = await _unitOfWork.TransactionsRepository.GetTotalTransactionAmountByUserId(userBo.UserId);
                var totalTransactionAmountBo = _mapper.Map<TotalTransactionAmountBo>(totalTransactionAmountDto);

                var totalExpence = totalTransactionAmountBo.TotalExpence;
                spentPercentage = (totalExpence / budgetBo.BudgetAmount) * 100;
            }    

            var subject = ApplicationConstsants.BudgetMailSubject;
            var body = ApplicationConstsants.GenerateBudgetSetEmailBody(userBo.FirstName, budgetBo.BudgetAmount, spentPercentage, (Months)(budgetBo.Month-1), budgetBo.Year);

            await _emailService.SendEmail(userBo.Email, subject, body);
        }
    }
}
