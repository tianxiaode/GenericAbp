using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.FileManagement.DataSeeds;

public class ResourceDataSeedContributor(IResourceDataSeed resourceDataSeed)
    : IDataSeedContributor, ITransientDependency
{
    protected IResourceDataSeed ResourceDataSeed { get; } = resourceDataSeed;

    public virtual async Task SeedAsync(DataSeedContext context)
    {
        await ResourceDataSeed.SeedAsync(context?.TenantId);
    }
}