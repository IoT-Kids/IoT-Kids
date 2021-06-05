using IoT_Kids.Repositories.IRepositories.IMembers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IoT_Kids.ScheduledTasks
{
    public class ConsumeScTask : BackgroundService
    {
       // private readonly ILogger<ConsumeScTask> _logger;
      // private readonly IMemberRepo _member;
      

        public ConsumeScTask(IServiceProvider services,
            ILogger<ConsumeScTask> logger)
        {
            Services = services;
          //  _logger = logger;
            //_member = member;
        }

        public IServiceProvider Services { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
         //   _logger.LogInformation(
           //     "Consume Scoped Service Hosted Service running.");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
           // _logger.LogInformation(
           //     "Consume Scoped Service Hosted Service is working.");

            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IMemberRepo>();

              //  await scopedProcessingService.SetStatusExpired();
                // await scopedProcessingService.DoWork(stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
          //  _logger.LogInformation(
          //      "Consume Scoped Service Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
 