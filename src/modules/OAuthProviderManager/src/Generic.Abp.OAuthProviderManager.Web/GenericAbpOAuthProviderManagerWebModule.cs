using Generic.Abp.OAuthProviderManager.Permissions;
using Generic.Abp.OAuthProviderManager.Web.Navigation;
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

namespace Generic.Abp.OAuthProviderManager.Web;

[DependsOn(
    typeof(GenericAbpOAuthProviderManagerHttpApiModule),
    typeof(AbpPermissionManagementWebModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreMvcUiThemeSharedModule)
    )]
public class GenericAbpOAuthProviderManagerWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(OAuthProviderManagerResource), typeof(GenericAbpOAuthProviderManagerWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpOAuthProviderManagerWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new OAuthProviderManagerMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options => { options.FileSets.AddEmbedded<GenericAbpOAuthProviderManagerWebModule>(); });

        context.Services.AddAutoMapperObjectMapper<GenericAbpOAuthProviderManagerWebModule>();
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<GenericAbpOAuthProviderManagerWebModule>(validate: true); });

        Configure<RazorPagesOptions>(options =>
        {
            //Configure authorization.

        });
    }

}
