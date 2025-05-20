using AutoMapper;
using PFMS.API.Models;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.Utils.Enums;

namespace PFMS.API.CronJobs
{
    public class RecurringTransactionJob: BackgroundService
    {
        private readonly TimeSpan _timeSpan;
        private readonly ILogger<RecurringTransactionJob> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;
        public RecurringTransactionJob(ILogger<RecurringTransactionJob> logger, IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _timeSpan = new TimeSpan(4, 0, 0);
            _logger = logger;
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while(cancellationToken.IsCancellationRequested == false)
            {
                DateTime now = DateTime.Now;
                TimeSpan delay;
                if(now > DateTime.Today.Add(_timeSpan))
                {
                    delay = DateTime.Today.AddDays(1).Add(_timeSpan) - now;
                }
                else
                {
                    delay = DateTime.Today.Add(_timeSpan) - now;
                }

                await Task.Delay(delay);

                if(cancellationToken.IsCancellationRequested == false)
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        IRecurringTransactionsService recurringTransactionsService = scope.ServiceProvider.GetRequiredService<IRecurringTransactionsService>();
                        ITransactionService transactionsService = scope.ServiceProvider.GetRequiredService<ITransactionService>();
                        IUserService userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                        List<RecurringTransactionBo> recurringTransactionBos = await recurringTransactionsService.GetRecurringTransactionsForToday();

                        List<RecurringTransactionResponseModel> recurringTransactionModels = _mapper.Map<List<RecurringTransactionResponseModel>>(recurringTransactionBos);

                        foreach(var model in recurringTransactionModels)
                        {
                            TransactionBo transactionBo = new TransactionBo()
                            {
                                Id = Guid.NewGuid(),
                                TransactionName = model.TransactionName,
                                TransactionDescription = model.TransactionDescription,
                                TransactionAmount = model.TransactionAmount,
                                TransactionType = model.TransactionType,
                                TransactionCategoryId = model.TransactionCategoryId,
                                TransactionDate = DateTime.Now,
                                TotalTransactionAmountId = (await userService.GetTotalTransactionAmountByUserId(model.UserId))!.Id
                            };
                            await transactionsService.AddTransaction(transactionBo, model.UserId, null, null);

                            if(model.TransactionInterval == TransactionInterval.Daily)
                            {
                                model.NextTransactionDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
                            }
                            else if(model.TransactionInterval == TransactionInterval.Monthly)
                            {
                                model.NextTransactionDate = DateOnly.FromDateTime(DateTime.Today.AddMonths(1));
                            }
                            model.LastTransactionDate = DateOnly.FromDateTime(DateTime.Today);

                            RecurringTransactionBo recurringTransactionBo = _mapper.Map<RecurringTransactionBo>(model);

                            await recurringTransactionsService.UpdateRecurringTransaction(recurringTransactionBo, recurringTransactionBo.Id,
                                recurringTransactionBo.UserId);
                        }
                    }
                }
            }
        }
    }
}
