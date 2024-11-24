using Generic.Abp.FileManagement.FileInfoBases;
using System.Threading.Tasks;
using Volo.Abp.DistributedLocking;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.FileManagement.Jobs;

public class DefaultRetentionPolicyUpdateJob(
    ISettingManager settingManager,
    IFileInfoBaseRepository repository,
    IAbpDistributedLock distributedLock)
    : JobBase<DefaultRetentionPolicyUpdateJobArgs>(settingManager, repository,
        distributedLock)
{
    protected override async Task StartAsync(DefaultRetentionPolicyUpdateJobArgs args)
    {
        await Repository.BulkUpdateExpireAtAsync(FileRetentionPolicy.Default, args.RetentionPeriod, args.BatchSize);
    }
}