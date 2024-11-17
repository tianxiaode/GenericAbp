using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.FileManagement.DataSeeds;

public class FolderDataSeedContributor(IFolderDataSeed folderDataSeed) : IDataSeedContributor, ITransientDependency
{
    protected IFolderDataSeed FolderDataSeed { get; } = folderDataSeed;

    public virtual async Task SeedAsync(DataSeedContext context)
    {
        await FolderDataSeed.SeedAsync(context?.TenantId);
    }
}