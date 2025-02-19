namespace PFMS.API.CronJobs
{
    public class SendTransactionNotificationJob: BackgroundService
    {
        private readonly TimeSpan _timeSpan;
        public SendTransactionNotificationJob()
        {
            _timeSpan = new TimeSpan(8, 0, 0);
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

                await Task.Delay(delay);
            }
        }
    }
}
