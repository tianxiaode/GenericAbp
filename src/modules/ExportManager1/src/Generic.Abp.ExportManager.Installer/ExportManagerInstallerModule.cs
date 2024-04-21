using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.ExportManager;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class ExportManagerInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ExportManagerInstallerModule>();
        });
    }
}
