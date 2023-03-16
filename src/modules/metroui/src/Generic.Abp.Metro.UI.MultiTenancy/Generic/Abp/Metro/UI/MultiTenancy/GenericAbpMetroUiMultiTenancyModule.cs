using Generic.Abp.Metro.UI.MultiTenancy.Localization;
using Generic.Abp.Metro.UI.Theme.Shared;
using Generic.Abp.Metro.UI.Theme.Shared.Bundling;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Metro.UI.MultiTenancy;

[DependsOn(
    typeof(GenericAbpMetroUiThemeSharedModule),
    typeof(AbpAspNetCoreMultiTenancyModule)
)]
public class GenericAbpMetroUiMultiTenancyModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(MetroUiMultiTenancyResource),
                typeof(GenericAbpMetroUiMultiTenancyModule).Assembly
            );
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpMetroUiMultiTenancyModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GenericAbpMetroUiMultiTenancyModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<MetroUiMultiTenancyResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Generic/Abp/Metro/UI/MultiTenancy/Localization/MultiTenancy");
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles
                .Get(StandardBundles.Scripts.Global)
                .AddFiles(
                    "/Pages/Abp/MultiTenancy/tenant-switch.js"
                );
        });
    }
}