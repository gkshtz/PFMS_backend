using AutoMapper;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.DAL.DTOs;
using PFMS.DAL.Interfaces;
using PFMS.Utils.Constants;
using PFMS.Utils.CustomExceptions;
using PFMS.Utils.Enums;
namespace PFMS.BLL.Services
{
    public class BudgetsService: IBudgetsService
    {
        private readonly IBudgetsRepository _budgetRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        public BudgetsService(IBudgetsRepository budgetRepository, IUserRepository userRepository, ITransactionRepository transactionRepository,IMapper mapper, IEmailService emailService)
        {
            _budgetRepository = budgetRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _emailService = emailService;
        }
        public async Task AddNewBudget(BudgetBo budgetBo, Guid userId)
        {
            var userDto = await _userRepository.GetUserById(userId);
            if(userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            var userBo = _mapper.Map<UserBo>(userDto);

            budgetBo.BudgetId = Guid.NewGuid();
            budgetBo.UserId = userId;

            var budgetDto = _mapper.Map<BudgetDto>(budgetBo);

            await _budgetRepository.AddBudget(budgetDto);

            await SendMailForBudgetSet(userBo, budgetBo);
        }

        private async Task SendMailForBudgetSet(UserBo userBo, BudgetBo budgetBo)
        {
            var totalTransactionAmountDto = await _transactionRepository.GetTotalTransactionAmountByUserId(userBo.UserId);
            var totalTransactionAmountBo = _mapper.Map<TotalTransactionAmountBo>(totalTransactionAmountDto);

            var totalExpence = totalTransactionAmountBo.TotalExpence;
            var spentPercentage = (totalExpence / budgetBo.BudgetAmount) * 100;

            var subject = ApplicationConstsants.BudgetMailSubject;
            var body = ApplicationConstsants.GenerateBudgetSetEmailBody(userBo.FirstName, budgetBo.BudgetAmount, spentPercentage, (Months)budgetBo.Month, budgetBo.Year);

            await _emailService.SendEmail(userBo.Email, subject, body);
        }
    }
}
