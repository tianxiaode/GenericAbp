using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Abp.FileManagement.Jobs;

public interface ITaskScheduler
{
    Task ScheduleAsync(string taskName, Func<Task> task, TimeSpan interval, CancellationToken cancellationToken);

    Task RescheduleAsync(string taskName, Func<Task> task, TimeSpan interval,
        CancellationToken cancellationToken);

    Task CancelAsync(string taskName); // 取消任务
    IEnumerable<string> GetScheduledTaskNames(); // 获取所有调度的任务名称
}