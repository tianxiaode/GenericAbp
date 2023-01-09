using Generic.Abp.PhoneLogin.Permissions;
using Generic.Abp.PhoneLogin.Web.Navigation;
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

namespace Generic.Abp.PhoneLogin.Web;

[DependsOn(
    typeof(GenericAbpPhoneLoginHttpApiModule),
    typeof(AbpPermissionManagementWebModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreMvcUiThemeSharedModule)
    )]
public class GenericAbpPhoneLoginWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(PhoneLoginResource), typeof(GenericAbpPhoneLoginWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpPhoneLoginWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new PhoneLoginMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options => { options.FileSets.AddEmbedded<GenericAbpPhoneLoginWebModule>(); });

        context.Services.AddAutoMapperObjectMapper<GenericAbpPhoneLoginWebModule>();
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<GenericAbpPhoneLoginWebModule>(validate: true); });

        Configure<RazorPagesOptions>(options =>
        {
            //Configure authorization.

        });
    }

}
