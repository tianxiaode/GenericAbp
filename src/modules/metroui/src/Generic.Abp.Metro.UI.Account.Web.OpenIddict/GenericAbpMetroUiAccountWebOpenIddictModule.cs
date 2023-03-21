using Generic.Abp.OpenIddict;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Metro.UI.Account.Web;

[DependsOn(
    typeof(GenericAbpMetroUiAccountWebModule),
    typeof(GenericAbpOpenIddictAspNetCoreModule)
)]
public class GenericAbpMetroUiAccountWebOpenIddictModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpMetroUiAccountWebOpenIddictModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GenericAbpMetroUiAccountWebOpenIddictModule>();
        });
    }
}