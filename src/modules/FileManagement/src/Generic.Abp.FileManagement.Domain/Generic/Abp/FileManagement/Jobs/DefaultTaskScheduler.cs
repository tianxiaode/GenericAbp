using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Generic.Abp.FileManagement.Jobs;

public class DefaultTaskScheduler(ILogger<DefaultTaskScheduler> logger) : ITaskScheduler, ITransientDependency
{
    protected ConcurrentDictionary<string, AbpAsyncTimer> Timers = new();
    protected ILogger<DefaultTaskScheduler> Logger { get; } = logger;

    public virtual Task ScheduleAsync(string taskName, Func<Task> task, TimeSpan interval,
        CancellationToken cancellationToken)
    {
        if (Timers.ContainsKey(taskName))
        {
            throw new InvalidOperationException($"Task '{taskName}' is already scheduled.");
        }

        var timer = new AbpAsyncTimer
        {
            Period = (int)interval.TotalMilliseconds,
            RunOnStart = true,
            Elapsed = _ =>
            {
                Logger.LogInformation($"Executing task '{taskName}'.");
                return task();
            }
        };

        Timers[taskName] = timer;
        timer.Start(cancellationToken);
        Logger.LogInformation($"Task '{taskName}' scheduled with interval of {interval.TotalSeconds} seconds.");
        return Task.CompletedTask;
    }

    public virtual async Task RescheduleAsync(string taskName, Func<Task> task, TimeSpan interval,
        CancellationToken cancellationToken)
    {
        await CancelAsync(taskName);
        await ScheduleAsync(taskName, task, interval, cancellationToken);
    }

    public virtual Task CancelAsync(string taskName)
    {
        if (Timers.TryRemove(taskName, out var timer))
        {
            timer.Stop();
            DisposeTimer(timer);
            Logger.LogInformation($"Task '{taskName}' has been unscheduled.");
        }
        else
        {
            Logger.LogWarning($"Attempt to cancel non-existent task '{taskName}'.");
        }

        return Task.CompletedTask;
    }

    public virtual void Dispose()
    {
        foreach (var timer in Timers.Values)
        {
            timer.Stop();
            DisposeTimer(timer);
        }

        Timers.Clear();
        Logger.LogInformation("All tasks have been unscheduled and timers disposed.");
    }

    private static void DisposeTimer(AbpAsyncTimer timer)
    {
        var disposableTimer = typeof(AbpAsyncTimer)
            .GetField("_taskTimer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.GetValue(timer) as IDisposable;
        disposableTimer?.Dispose();
    }
}