using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IoT_Kids.ScheduledTasks
{
    public interface IScTask
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}
