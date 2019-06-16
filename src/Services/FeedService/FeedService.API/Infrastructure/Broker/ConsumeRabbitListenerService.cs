using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FeedService.Infrastructure.Broker
{
    internal class ConsumeRabbitListenerService : IHostedService
    {
        private readonly ILogger _logger;

        public ConsumeRabbitListenerService(IServiceProvider services, 
            ILogger<ConsumeRabbitListenerService> logger)
        {
            Services = services;
            _logger = logger;
        }

        public IServiceProvider Services { get; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is starting.");

            DoWork();

            return Task.CompletedTask;
        }

        private void DoWork()
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is working.");

            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService = 
                    scope.ServiceProvider
                        .GetRequiredService<IRabbitListenerService>();
                
                scopedProcessingService.DoWork(Services);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            return Task.CompletedTask;
        }
    }
}