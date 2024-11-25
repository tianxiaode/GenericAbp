using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SettingManagement;
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
            var defaultFileSetting = await FileManagementSettingManager.GetDefaultFileSettingAsync();
            var temporaryFileSetting = await FileManagementSettingManager.GetTemporaryFileSettingAsync();
            await ScheduleOrUpdateTaskAsync("DefaultFileUpdate", tenantId,
                defaultFileSetting.UpdateSetting.Enable,
                defaultFileSetting.UpdateSetting.Frequency,
                () => ExecuteDefaultUpdateAsync(tenantId, defaultFileSetting.UpdateSetting.RetentionPeriod,
                    defaultFileSetting.UpdateSetting.BatchSize));

            await ScheduleOrUpdateTaskAsync("DefaultCleanup", tenantId,
                defaultFileSetting.CleanupSetting.Enable,
                defaultFileSetting.CleanupSetting.Frequency,
                () => ExecuteDefaultCleanupAsync(tenantId, defaultFileSetting.CleanupSetting.BatchSize));

            await ScheduleOrUpdateTaskAsync("TemporaryUpdate", tenantId,
                temporaryFileSetting.UpdateSetting.Enable,
                temporaryFileSetting.UpdateSetting.Frequency,
                () => ExecuteTemporaryUpdateAsync(tenantId, temporaryFileSetting.UpdateSetting.RetentionPeriod,
                    temporaryFileSetting.UpdateSetting.BatchSize));

            await ScheduleOrUpdateTaskAsync("TemporaryCleanup", tenantId,
                temporaryFileSetting.CleanupSetting.Enable,
                temporaryFileSetting.CleanupSetting.Frequency,
                () => ExecuteTemporaryCleanupAsync(tenantId, temporaryFileSetting.CleanupSetting.BatchSize));
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
        int frequency,
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

    protected virtual async Task ExecuteDefaultUpdateAsync(Guid? tenantId, int retentionPeriod, int batchSize)
    {
        try
        {
            var jobName =
                await GetJobNameAsync(tenantId, nameof(DefaultRetentionPolicyUpdateJob) + ":" + "TemporaryFile");
            await BackgroundJobManager.EnqueueAsync(tenantId, jobName,
                new DefaultRetentionPolicyUpdateJobArgs(tenantId, jobName, retentionPeriod, batchSize));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error executing DefaultUpdate for tenant {TenantId}", tenantId);
        }
    }

    protected virtual async Task ExecuteDefaultCleanupAsync(Guid? tenantId, int batchSize)
    {
        try
        {
            var jobName = await GetJobNameAsync(tenantId, nameof(DefaultFileCleanupJob) + ":" + "DefaultFile");
            await BackgroundJobManager.EnqueueAsync(tenantId, jobName,
                new DefaultFileCleanupJobArgs(tenantId, jobName, batchSize));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error executing DefaultCleanup for tenant {TenantId}", tenantId);
        }
    }

    protected virtual async Task ExecuteTemporaryUpdateAsync(Guid? tenantId, int retentionPeriod, int batchSize)
    {
        try
        {
            var jobName =
                await GetJobNameAsync(tenantId, nameof(DefaultRetentionPolicyUpdateJob) + ":" + "TemporaryFile");
            await BackgroundJobManager.EnqueueAsync(tenantId, jobName,
                new DefaultRetentionPolicyUpdateJobArgs(tenantId, jobName, retentionPeriod, batchSize));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error executing TemporaryUpdate for tenant {TenantId}", tenantId);
        }
    }

    protected virtual async Task ExecuteTemporaryCleanupAsync(Guid? tenantId, int batchSize)
    {
        try
        {
            var jobName = await GetJobNameAsync(tenantId, nameof(DefaultFileCleanupJob) + ":" + "TemporaryFile");
            await BackgroundJobManager.EnqueueAsync(tenantId, jobName,
                new DefaultFileCleanupJobArgs(tenantId, jobName, batchSize));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error executing TemporaryCleanup for tenant {TenantId}", tenantId);
        }
    }

    protected virtual Task<string> GetJobNameAsync(Guid? tenantId, string jobName)
    {
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