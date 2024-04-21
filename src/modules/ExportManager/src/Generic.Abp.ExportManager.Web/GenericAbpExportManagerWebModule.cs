using Generic.Abp.ExportManager.Permissions;
using Generic.Abp.ExportManager.Web.Navigation;
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

namespace Generic.Abp.ExportManager.Web;

[DependsOn(
    typeof(GenericAbpExportManagerHttpApiModule),
    typeof(AbpPermissionManagementWebModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreMvcUiThemeSharedModule)
    )]
public class GenericAbpExportManagerWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(ExportManagerResource), typeof(GenericAbpExportManagerWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpExportManagerWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new ExportManagerMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options => { options.FileSets.AddEmbedded<GenericAbpExportManagerWebModule>(); });

        context.Services.AddAutoMapperObjectMapper<GenericAbpExportManagerWebModule>();
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<GenericAbpExportManagerWebModule>(validate: true); });

        Configure<RazorPagesOptions>(options =>
        {
            //Configure authorization.

        });
    }

}
