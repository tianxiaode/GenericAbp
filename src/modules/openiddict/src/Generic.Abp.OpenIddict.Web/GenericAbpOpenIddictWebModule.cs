using Generic.Abp.OpenIddict.Permissions;
using Generic.Abp.OpenIddict.Web.Menus;
using Generic.Abp.W2Ui;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AutoMapper;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Microsoft.Extensions.DependencyInjection;
using Generic.Abp.OpenIddict.Localization;

namespace Generic.Abp.OpenIddict.Web;

[DependsOn(
    typeof(AbpPermissionManagementWebModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreMvcUiThemeSharedModule),
    typeof(GenericAbpOpenIddictApplicationContractsModule),
        typeof(GenericAbpW2UiModule)
    )]
public class GenericAbpOpenIddictWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(OpenIddictResource), typeof(GenericAbpOpenIddictWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpOpenIddictWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new OpenIddictMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options => { options.FileSets.AddEmbedded<GenericAbpOpenIddictWebModule>(); });

        context.Services.AddAutoMapperObjectMapper<GenericAbpOpenIddictWebModule>();
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<GenericAbpOpenIddictWebModule>(validate: true); });

        Configure<RazorPagesOptions>(options =>
        {
            //Configure authorization.
            options.Conventions.AuthorizePage("/OpenIddict/Applications/Index", OpenIddictPermissions.Applications.Default);
            options.Conventions.AuthorizePage("/OpenIddict/ApiResources/CreateModal", OpenIddictPermissions.Applications.Create);
            options.Conventions.AuthorizePage("/OpenIddict/ApiResources/EditModal", OpenIddictPermissions.Applications.Update);

            options.Conventions.AuthorizePage("/OpenIddict/Scopes/Index", OpenIddictPermissions.Scopes.Default);
            options.Conventions.AuthorizePage("/OpenIddict/Scopes/CreateModal", OpenIddictPermissions.Scopes.Create);
            options.Conventions.AuthorizePage("/OpenIddict/Scopes/EditModal", OpenIddictPermissions.Scopes.Update);



        });
    }

}
