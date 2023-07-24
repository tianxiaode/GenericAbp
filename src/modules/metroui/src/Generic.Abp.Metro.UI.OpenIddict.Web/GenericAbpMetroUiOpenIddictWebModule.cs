using Generic.Abp.Metro.UI.OpenIddict.Web.Menus;
using Generic.Abp.Metro.UI.Theme.Shared;
using Generic.Abp.OpenIddict;
using Generic.Abp.OpenIddict.Localization;
using Generic.Abp.OpenIddict.Permissions;
using Generic.Abp.W2Ui;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Metro.UI.OpenIddict.Web;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(GenericAbpMetroUiThemeSharedModule),
    typeof(GenericAbpOpenIddictApplicationContractsModule),
    typeof(GenericAbpW2UiModule)
)]
public class GenericAbpMetroUiOpenIddictWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(OpenIddictResource),
                typeof(GenericAbpMetroUiOpenIddictWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpMetroUiOpenIddictWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options => { options.MenuContributors.Add(new OpenIddictMenuContributor()); });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GenericAbpMetroUiOpenIddictWebModule>();
        });

        context.Services.AddAutoMapperObjectMapper<GenericAbpMetroUiOpenIddictWebModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<GenericAbpMetroUiOpenIddictWebModule>(validate: true);
        });

        Configure<RazorPagesOptions>(options =>
        {
            //Configure authorization.
            options.Conventions.AuthorizePage("/OpenIddict/Applications/Index",
                OpenIddictPermissions.Applications.Default);
            options.Conventions.AuthorizePage("/OpenIddict/ApiResources/CreateModal",
                OpenIddictPermissions.Applications.Create);
            options.Conventions.AuthorizePage("/OpenIddict/ApiResources/EditModal",
                OpenIddictPermissions.Applications.Update);

            options.Conventions.AuthorizePage("/OpenIddict/Scopes/Index", OpenIddictPermissions.Scopes.Default);
            options.Conventions.AuthorizePage("/OpenIddict/Scopes/CreateModal", OpenIddictPermissions.Scopes.Create);
            options.Conventions.AuthorizePage("/OpenIddict/Scopes/EditModal", OpenIddictPermissions.Scopes.Update);
        });
    }
}