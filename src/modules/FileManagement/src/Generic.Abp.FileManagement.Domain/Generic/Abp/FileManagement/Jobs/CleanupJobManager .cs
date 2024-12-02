using Generic.Abp.FileManagement.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Generic.Abp.FileManagement.Jobs;

public class CleanupJobManager(
    ITaskScheduler taskScheduler,
    ILogger<CleanupJobManager> logger,
    DefaultFileManagementBackgroundJobManager backgroundJobManager,
    FileManagementSettingManager settingManager,
    ICancellationTokenProvider cancellationTokenProvider) : ITransientDependency
{
    protected ITaskScheduler TaskScheduler { get; } = taskScheduler;
    protected ILogger<CleanupJobManager> Logger { get; } = logger;
    protected DefaultFileManagementBackgroundJobManager BackgroundJobManager { get; } = backgroundJobManager;
    protected FileManagementSettingManager FileManagementSettingManager { get; } = settingManager;
    protected ICancellationTokenProvider CancellationTokenProvider { get; } = cancellationTokenProvider;
    protected CancellationToken CancellationToken => CancellationTokenProvider.Token;

    public virtual async Task StartAsync(Guid? tenantId)
    {
        try
        {
            var settings = await FileManagementSettingManager.GetAllCleanupOrUpdateSettingsAsync();
            foreach (var (key, cleanupOrUpdateSetting) in settings)
            {
                await ScheduleOrUpdateTaskAsync(key, tenantId,
                    cleanupOrUpdateSetting.Enable,
                    cleanupOrUpdateSetting.Frequency,
                    () => ExecuteAsync(key, tenantId, cleanupOrUpdateSetting.RetentionPeriod,
                        cleanupOrUpdateSetting.BatchSize));
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error applying settings for tenant {TenantId}", tenantId);
        }
    }

    public virtual async Task StopAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            // 假设你有一个跟踪已调度任务的列表或字典
            var allTaskNames = GetAllScheduledTaskNames();

            foreach (var taskName in allTaskNames)
            {
                Logger.LogInformation($"Cancelling task: {taskName}");
                await TaskScheduler.CancelAsync(taskName); // 取消任务
            }

            Logger.LogInformation("All scheduled tasks have been cancelled.");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error occurred while stopping all tasks.");
        }
    }

    private IEnumerable<string> GetAllScheduledTaskNames()
    {
        // 返回一个所有已调度任务的名字的集合
        // 你可以从 TaskScheduler 中获取当前的任务名称列表（例如从字典 Timers 中）
        return TaskScheduler.GetScheduledTaskNames(); // 假设 TaskScheduler 有此方法
    }

    protected virtual async Task ScheduleOrUpdateTaskAsync(
        string taskPrefix,
        Guid? tenantId,
        bool enable,
        long frequency,
        Func<Task> task)
    {
        var taskName = $"{taskPrefix}:{tenantId}";
        var interval =
            TimeSpan.FromMinutes(frequency);

        await HandleTaskAsync(taskName, enable, interval, task);
    }

    protected virtual async Task HandleTaskAsync(string taskName, bool enable, TimeSpan interval, Func<Task> task)
    {
        if (enable)
        {
            Logger.LogInformation($"Scheduling task: {taskName}");
            await TaskScheduler.RescheduleAsync(taskName, task, interval, CancellationToken);
        }
        else
        {
            Logger.LogInformation($"Cancelling task: {taskName}");
            await TaskScheduler.CancelAsync(taskName);
        }
    }

    protected virtual async Task ExecuteAsync(string prefix, Guid? tenantId, int? retentionPeriod, int batchSize)
    {
        try
        {
            var jobName =
                await GetJobNameAsync(tenantId, prefix, nameof(DefaultRetentionPolicyUpdateJob));
            if (retentionPeriod.HasValue)
            {
                await BackgroundJobManager.EnqueueAsync(tenantId, jobName,
                    new DefaultRetentionPolicyUpdateJobArgs(tenantId, jobName, retentionPeriod, batchSize));
            }
            else
            {
                await BackgroundJobManager.EnqueueAsync(tenantId, jobName,
                    new DefaultFileCleanupJobArgs(tenantId, jobName, batchSize));
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error executing DefaultUpdate for tenant {TenantId}", tenantId);
        }
    }

    protected virtual Task<string> GetJobNameAsync(Guid? tenantId, string prefix, string jobName)
    {
        jobName = $"{prefix}:{jobName}";
        if (tenantId.HasValue)
        {
            jobName += $"-{tenantId.Value}";
        }

        return Task.FromResult(jobName);
    }
}


/*
 *
 * public class HangFireTaskScheduler : ITaskScheduler
   {
       public Task ScheduleAsync(string taskName, Func<Task> task, TimeSpan interval, CancellationToken cancellationToken)
       {
           RecurringJob.AddOrUpdate(taskName, () => task(), Cron.MinuteInterval((int)interval.TotalMinutes));
           return Task.CompletedTask;
       }

       public Task CancelAsync(string taskName)
       {
           RecurringJob.RemoveIfExists(taskName);
           return Task.CompletedTask;
       }
   }
   *
 */

/*
 *
 * public class QuartzTaskScheduler : ITaskScheduler
   {
       private readonly IScheduler _scheduler;

       public QuartzTaskScheduler(IScheduler scheduler)
       {
           _scheduler = scheduler;
       }

       public async Task ScheduleAsync(string taskName, Func<Task> task, TimeSpan interval, CancellationToken cancellationToken)
       {
           var job = JobBuilder.Create<QuartzJobWrapper>()
               .WithIdentity(taskName)
               .Build();

           job.JobDataMap["Task"] = task;

           var trigger = TriggerBuilder.Create()
               .WithIdentity(taskName + "-Trigger")
               .StartNow()
               .WithSimpleSchedule(x => x.WithInterval(interval).RepeatForever())
               .Build();

           await _scheduler.ScheduleJob(job, trigger, cancellationToken);
       }

       public async Task CancelAsync(string taskName)
       {
           var jobKey = new JobKey(taskName);
           if (await _scheduler.CheckExists(jobKey))
           {
               await _scheduler.DeleteJob(jobKey);
           }
       }
   }
   */