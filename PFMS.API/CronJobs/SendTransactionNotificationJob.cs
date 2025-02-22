using PFMS.BLL.Interfaces;

namespace PFMS.API.CronJobs
{
    public class SendTransactionNotificationJob: BackgroundService
    {
        private readonly TimeSpan _timeSpan;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SendTransactionNotificationJob> _logger;
        public SendTransactionNotificationJob(IServiceScopeFactory scopeFactory, ILogger<SendTransactionNotificationJob> logger)
        {
            _timeSpan = new TimeSpan(19, 37, 0);
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while(!cancellationToken.IsCancellationRequested)
            {
                DateTime now = DateTime.Now;
                TimeSpan delay;
                if(now > DateTime.Today.Add(_timeSpan))
                {
                    delay = DateTime.Today.Add(_timeSpan).AddDays(1) - now;
                }
                else
                {
                    delay = DateTime.Today.Add(_timeSpan) - now;
                }

                await Task.Delay(delay, cancellationToken);

                if(!cancellationToken.IsCancellationRequested)
                { 
                    try
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            ITransactionNotificationsService notificationsService = scope.ServiceProvider.GetRequiredService<ITransactionNotificationsService>();

                            await notificationsService.SendScheduledNotifications();
                        }
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError(ex, $"Log Id: {Guid.NewGuid()} - {ex.Message}");
                    }
                }
            }
        }
    }
}
