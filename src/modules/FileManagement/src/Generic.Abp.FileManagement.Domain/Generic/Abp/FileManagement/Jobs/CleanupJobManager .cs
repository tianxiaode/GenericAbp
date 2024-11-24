using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.Settings;
using Microsoft.Extensions.Logging;
using System;
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
    ISettingManager settingManager,
    ICancellationTokenProvider cancellationTokenProvider) : ITransientDependency
{
    protected ITaskScheduler TaskScheduler { get; } = taskScheduler;
    protected ILogger<CleanupJobManager> Logger { get; } = logger;
    protected DefaultFileManagementBackgroundJobManager BackgroundJobManager { get; } = backgroundJobManager;
    protected ISettingManager SettingManager { get; } = settingManager;
    protected ICancellationTokenProvider CancellationTokenProvider { get; } = cancellationTokenProvider;
    protected CancellationToken CancellationToken => CancellationTokenProvider.Token;

    public virtual async Task ApplySettingsAsync(Guid? tenantId)
    {
        try
        {
            await ScheduleOrUpdateTaskAsync("DefaultUpdate", tenantId,
                FileManagementSettings.Default.UpdateEnable,
                FileManagementSettings.Default.UpdateFrequency,
                ResourceConsts.Default.UpdateFrequency,
                () => ExecuteDefaultUpdateAsync(tenantId));

            await ScheduleOrUpdateTaskAsync("DefaultCleanup", tenantId,
                FileManagementSettings.Default.CleanupEnable,
                FileManagementSettings.Default.CleanupFrequency,
                ResourceConsts.Default.CleanupFrequency,
                () => ExecuteDefaultCleanupAsync(tenantId));

            await ScheduleOrUpdateTaskAsync("TemporaryUpdate", tenantId,
                FileManagementSettings.Temporary.UpdateEnable,
                FileManagementSettings.Temporary.UpdateFrequency,
                ResourceConsts.Temporary.UpdateFrequency,
                () => ExecuteTemporaryUpdateAsync(tenantId));

            await ScheduleOrUpdateTaskAsync("TemporaryCleanup", tenantId,
                FileManagementSettings.Temporary.CleanupEnable,
                FileManagementSettings.Temporary.CleanupFrequency,
                ResourceConsts.Temporary.CleanupFrequency,
                () => ExecuteTemporaryCleanupAsync(tenantId));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error applying settings for tenant {TenantId}", tenantId);
        }
    }

    protected virtual async Task ScheduleOrUpdateTaskAsync(
        string taskPrefix,
        Guid? tenantId,
        string enableSettingKey,
        string frequencySettingKey,
        int defaultFrequency,
        Func<Task> task)
    {
        var taskName = $"{taskPrefix}:{tenantId}";
        var enable = await FileManagementConfigurationHelper.GetEnableAsync(SettingManager, enableSettingKey);
        var interval =
            TimeSpan.FromMinutes(await FileManagementConfigurationHelper.GetFrequencyAsync(SettingManager,
                frequencySettingKey, defaultFrequency));

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

    protected virtual async Task ExecuteDefaultUpdateAsync(Guid? tenantId)
    {
        try
        {
            var jobName =
                await GetJobNameAsync(tenantId, nameof(DefaultRetentionPolicyUpdateJob) + ":" + "TemporaryFile");
            await BackgroundJobManager.EnqueueAsync(tenantId,
                jobName,
                new DefaultRetentionPolicyUpdateJobArgs(tenantId, jobName,
                    await FileManagementConfigurationHelper.GetRetentionPeriodAsync(SettingManager,
                        FileManagementSettings.Default.UpdateRetentionPeriod,
                        ResourceConsts.Default.UpdateRetentionPeriod),
                    await FileManagementConfigurationHelper.GetBatchSizeAsync(SettingManager,
                        FileManagementSettings.Default.UpdateBatchSize, ResourceConsts.Default.UpdateBatchSize)
                ));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error executing DefaultUpdate for tenant {TenantId}", tenantId);
        }
    }

    protected virtual async Task ExecuteDefaultCleanupAsync(Guid? tenantId)
    {
        try
        {
            var jobName = await GetJobNameAsync(tenantId, nameof(DefaultFileCleanupJob) + ":" + "DefaultFile");
            await BackgroundJobManager.EnqueueAsync(tenantId,
                jobName,
                new DefaultFileCleanupJobArgs(tenantId, jobName,
                    await FileManagementConfigurationHelper.GetBatchSizeAsync(SettingManager,
                        FileManagementSettings.Default.CleanupBatchSize, ResourceConsts.Default.CleanupBatchSize)
                ));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error executing DefaultCleanup for tenant {TenantId}", tenantId);
        }
    }

    protected virtual async Task ExecuteTemporaryUpdateAsync(Guid? tenantId)
    {
        try
        {
            var jobName =
                await GetJobNameAsync(tenantId, nameof(DefaultRetentionPolicyUpdateJob) + ":" + "TemporaryFile");
            await BackgroundJobManager.EnqueueAsync(tenantId,
                jobName,
                new DefaultRetentionPolicyUpdateJobArgs(tenantId, jobName,
                    await FileManagementConfigurationHelper.GetRetentionPeriodAsync(SettingManager,
                        FileManagementSettings.Temporary.UpdateRetentionPeriod,
                        ResourceConsts.Temporary.UpdateRetentionPeriod),
                    await FileManagementConfigurationHelper.GetBatchSizeAsync(SettingManager,
                        FileManagementSettings.Temporary.UpdateBatchSize, ResourceConsts.Temporary.UpdateBatchSize)
                ));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error executing TemporaryUpdate for tenant {TenantId}", tenantId);
        }
    }

    protected virtual async Task ExecuteTemporaryCleanupAsync(Guid? tenantId)
    {
        try
        {
            var jobName = await GetJobNameAsync(tenantId, nameof(DefaultFileCleanupJob) + ":" + "TemporaryFile");
            await BackgroundJobManager.EnqueueAsync(tenantId,
                jobName,
                new DefaultFileCleanupJobArgs(tenantId, jobName,
                    await FileManagementConfigurationHelper.GetBatchSizeAsync(SettingManager,
                        FileManagementSettings.Temporary.CleanupBatchSize, ResourceConsts.Temporary.CleanupBatchSize)
                ));
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