using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IoT_Kids.ScheduledTasks
{
    public class ScTask : IScTask
    {
        private int executionCount = 0;
        private readonly ILogger _logger;

        public ScTask(ILogger<ScTask> logger)
        {
            _logger = logger;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                executionCount++;

                _logger.LogInformation(
                    "Scoped Processing Service is working. Count: {Count}", executionCount);

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
