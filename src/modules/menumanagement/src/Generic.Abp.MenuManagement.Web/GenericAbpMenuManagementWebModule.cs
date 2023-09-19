using Generic.Abp.MenuManagement.Permissions;
using Generic.Abp.MenuManagement.Web.Navigation;
using Generic.Abp.W2Ui;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AutoMapper;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.MenuManagement.Web;

[DependsOn(
    typeof(GenericAbpMenuManagementHttpApiModule),
    typeof(AbpPermissionManagementWebModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreMvcUiThemeSharedModule)
    )]
public class GenericAbpMenuManagementWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(MenuManagementResource), typeof(GenericAbpMenuManagementWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpMenuManagementWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new MenuManagementMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options => { options.FileSets.AddEmbedded<GenericAbpMenuManagementWebModule>(); });

        context.Services.AddAutoMapperObjectMapper<GenericAbpMenuManagementWebModule>();
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<GenericAbpMenuManagementWebModule>(validate: true); });

        Configure<RazorPagesOptions>(options =>
        {
            //Configure authorization.

        });
    }

}
