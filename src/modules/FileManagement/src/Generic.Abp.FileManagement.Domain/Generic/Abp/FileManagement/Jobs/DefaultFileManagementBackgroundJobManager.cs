using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Timing;

namespace Generic.Abp.FileManagement.Jobs;

public class DefaultFileManagementBackgroundJobManager(
    IClock clock,
    IBackgroundJobSerializer serializer,
    IGuidGenerator guidGenerator,
    IBackgroundJobStore store)
    : IBackgroundJobManager, ITransientDependency
{
    protected IClock Clock { get; } = clock;
    protected IBackgroundJobSerializer Serializer { get; } = serializer;
    protected IGuidGenerator GuidGenerator { get; } = guidGenerator;
    protected IBackgroundJobStore Store { get; } = store;

    public Task<string> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal,
        TimeSpan? delay = null)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<Guid> EnqueueAsync(Guid? tenantId, string jobName, object args,
        BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null)
    {
        if (tenantId.HasValue)
        {
            jobName = $"{tenantId}:{jobName}";
        }

        var jobInfo = new BackgroundJobInfo
        {
            Id = GuidGenerator.Create(),
            JobName = jobName,
            JobArgs = Serializer.Serialize(args),
            Priority = priority,
            CreationTime = Clock.Now,
            NextTryTime = Clock.Now
        };

        if (delay.HasValue)
        {
            jobInfo.NextTryTime = Clock.Now.Add(delay.Value);
        }

        await Store.InsertAsync(jobInfo);

        return jobInfo.Id;
    }
}