using Hangfire;
using LibraryManagementSystem.Connetcion;

namespace LibraryManagementSystem.HostedService
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<TimedHostedService> _logger;
        private Timer? _timer = null;

        public TimedHostedService(ILogger<TimedHostedService> logger)
        {
            _logger = logger;
        }

        private void DoWork(object? state) 
        {         
               _logger.LogInformation("MyHostedService started at {time}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                MyQuery.BookReturnDateCheck();
            
        }

        public void DoWork2() 
        {
            _timer = new Timer(DoWork, null, 0, Timeout.Infinite);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var currentDate = DateTime.UtcNow;
            DateTime workingTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 12,43, 00);
            var date2 = workingTime.AddMinutes(1);
            var lateTime = workingTime - currentDate; 
            if (currentDate > workingTime)
            {
                _timer = new Timer(DoWork, null, 0,Timeout.Infinite);
            }

            else
            {     
                BackgroundJob.Schedule(() => DoWork2(), date2);
            }

            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("MyHostedService stopped at {time}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
