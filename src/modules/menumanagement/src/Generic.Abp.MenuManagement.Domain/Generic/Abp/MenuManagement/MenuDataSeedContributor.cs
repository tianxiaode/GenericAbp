using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.MenuManagement;

public class MenuDataSeedContributor(IMenuDataSeed menuDataSeed) : IDataSeedContributor, ITransientDependency
{
    protected IMenuDataSeed MenuDataSeed { get; } = menuDataSeed;

    public virtual async Task SeedAsync(DataSeedContext context)
    {
        await MenuDataSeed.SeedAsync(context?.TenantId);
    }
}