using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System;
using Reposatiory;

namespace Lofty.SchudleTask
{
    /// <summary>
    /// Schudele task for realase pending shares
    /// </summary>
    /// <param name="scopeFactory"></param>
    public class OrderConfirmationBackgroundService(IServiceScopeFactory scopeFactory) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
                using IServiceScope scope = _scopeFactory.CreateScope();
                var orderService = scope.ServiceProvider.GetRequiredService<OrderManager>();
                await orderService.RealasePendingSharesAsync();
            }
        }
    }
}
