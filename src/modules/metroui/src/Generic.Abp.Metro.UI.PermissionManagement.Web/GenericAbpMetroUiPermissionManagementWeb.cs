using Generic.Abp.Metro.UI.Theme.Shared;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AutoMapper;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Metro.UI.PermissionManagement.Web;

[DependsOn(typeof(AbpPermissionManagementApplicationContractsModule))]
[DependsOn(typeof(GenericAbpMetroUiThemeSharedModule))]
[DependsOn(typeof(AbpAutoMapperModule))]
public class GenericAbpMetroUiPermissionManagementWeb : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(AbpPermissionManagementResource));
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpMetroUiPermissionManagementWeb).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GenericAbpMetroUiPermissionManagementWeb>();
        });

        context.Services.AddAutoMapperObjectMapper<GenericAbpMetroUiPermissionManagementWeb>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AbpPermissionManagementWebAutoMapperProfile>(validate: true);
        });

        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(PermissionManagementRemoteServiceConsts.ModuleName);
        });
    }
}