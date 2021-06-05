using IoT_Kids.Repositories.IRepositories.IMembers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IoT_Kids.ScheduledTasks
{
    public class SetExpiredMemberStatus : IHostedService
    {
        public IServiceProvider Services { get; }
        private readonly IServiceProvider _services;
        private Timer _timer;
        public SetExpiredMemberStatus(IServiceProvider services)
        {
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(
            DoWork,
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(10)
);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            //SomeWork().GetAwaiter().GetResult();

            using (var scope = _services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IMemberRepo>();

                scopedProcessingService.SetStatusExpired().GetAwaiter().GetResult() ;
            }
        }

        public async Task SomeWork()
        {
            using (var scope = _services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IMemberRepo>();

              await  scopedProcessingService.SetStatusExpired();
            }
        }

    }
}
