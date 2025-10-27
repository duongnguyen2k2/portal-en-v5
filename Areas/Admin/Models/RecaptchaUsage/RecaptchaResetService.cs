using API.Areas.Admin.Models.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.RecaptchaUsage
{
    public class RecaptchaResetService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RecaptchaResetService> _logger;
        private readonly IConfiguration _config;
        private readonly int[] _validIdCoQuans;

        public RecaptchaResetService(IServiceProvider serviceProvider, ILogger<RecaptchaResetService> logger, IConfiguration config)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _config = config;

            // Đọc mảng ValidIdCoQuans từ config (fallback nếu không có hoặc empty)
            var validIdsSection = _config.GetSection("RecaptchaSettings:ValidIdCoQuans");
            _validIdCoQuans = validIdsSection.Get<int[]>() ?? new int[] { 0 };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var now = DateTime.UtcNow;  
            var resetDate = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);  

            while (!stoppingToken.IsCancellationRequested)
            {
                var nextReset = resetDate.AddMonths(1);  
                var delay = nextReset - DateTime.UtcNow;

                if (delay.TotalMilliseconds > 0)
                {
                    
                    await Task.Delay((int)delay.TotalMilliseconds, stoppingToken);
                }

                if (stoppingToken.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var quotaService = scope.ServiceProvider.GetRequiredService<IRecaptchaQuotaService>();
                    quotaService.ResetMonthlyUsage();  

                    _logger.LogInformation("reCAPTCHA quota reset for {Month}/{Year} - Valid IdCoQuans: [{Ids}]",
                        nextReset.Month, nextReset.Year, string.Join(",", _validIdCoQuans));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error resetting reCAPTCHA quota for {Month}/{Year}", nextReset.Month, nextReset.Year);
                }

                resetDate = nextReset;  
            }
        }
    }
}