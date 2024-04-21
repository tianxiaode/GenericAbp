using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.ExportManager;

[DependsOn(
    typeof(ExportManagerApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class ExportManagerHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(ExportManagerApplicationContractsModule).Assembly,
            ExportManagerRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ExportManagerHttpApiClientModule>();
        });

    }
}
