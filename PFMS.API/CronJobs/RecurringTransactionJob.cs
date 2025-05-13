using PFMS.BLL.Interfaces;

namespace PFMS.API.CronJobs
{
    public class RecurringTransactionJob: BackgroundService
    {
        private readonly TimeSpan _timeSpan;
        private readonly ILogger<RecurringTransactionJob> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        public RecurringTransactionJob(ILogger<RecurringTransactionJob> logger, IServiceScopeFactory scopeFactory)
        {
            _timeSpan = new TimeSpan(4, 0, 0);
            _logger = logger;
            _scopeFactory = scopeFactory;
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
                    }
                }
            }
        }
    }
}
