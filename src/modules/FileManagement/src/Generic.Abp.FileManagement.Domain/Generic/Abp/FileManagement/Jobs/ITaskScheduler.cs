using System;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Abp.FileManagement.Jobs;

public interface ITaskScheduler
{
    Task ScheduleAsync(string taskName, Func<Task> task, TimeSpan interval, CancellationToken cancellationToken);

    Task RescheduleAsync(string taskName, Func<Task> task, TimeSpan interval,
        CancellationToken cancellationToken);

    Task CancelAsync(string taskName);
}