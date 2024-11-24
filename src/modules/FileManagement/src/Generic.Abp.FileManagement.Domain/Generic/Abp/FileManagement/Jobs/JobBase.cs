using Generic.Abp.FileManagement.FileInfoBases;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.FileManagement.Jobs;

public class JobBase<TJobArgs>(
    ISettingManager settingManager,
    IFileInfoBaseRepository repository,
    IAbpDistributedLock distributedLock
) : AsyncBackgroundJob<TJobArgs>, ITransientDependency
    where TJobArgs : JobBaseArgs
{
    protected ISettingManager SettingManager { get; } = settingManager;
    protected IFileInfoBaseRepository Repository { get; } = repository;
    protected IAbpDistributedLock DistributedLock { get; } = distributedLock;


    public override async Task ExecuteAsync(TJobArgs args)
    {
        await using var handle =
            await DistributedLock.TryAcquireAsync(await GetLockNameAsync(args.TenantId, args.Name),
                TimeSpan.FromMinutes(10));
        if (handle == null)
        {
            Logger.LogWarning("Could not acquire lock for DefaultFileCleanupJob.");
            return;
        }

        await StartAsync(args);
    }

    protected virtual Task StartAsync(TJobArgs args)
    {
        return Task.CompletedTask;
    }

    protected virtual Task<string> GetLockNameAsync(Guid? tenantId, string name)
    {
        return Task.FromResult(tenantId.HasValue ? $"{name}:{tenantId}" : name);
    }
}