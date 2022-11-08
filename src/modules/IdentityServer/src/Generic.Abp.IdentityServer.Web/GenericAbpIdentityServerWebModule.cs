using Generic.Abp.IdentityServer.Permissions;
using Generic.Abp.IdentityServer.Web.Navigation;
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

namespace Generic.Abp.IdentityServer.Web
{
    [DependsOn(
        typeof(AbpPermissionManagementWebModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpAspNetCoreMvcUiThemeSharedModule),
        typeof(GenericAbpIdentityServerHttpApiModule),
        typeof(GenericAbpW2UiModule)
    )]
    public class GenericAbpIdentityServerWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(AbpIdentityServerResource), typeof(GenericAbpIdentityServerWebModule).Assembly);
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpIdentityServerWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new IdentityServerWebMainMenuContributor());
            });

            Configure<AbpVirtualFileSystemOptions>(options => { options.FileSets.AddEmbedded<GenericAbpIdentityServerWebModule>(); });

            context.Services.AddAutoMapperObjectMapper<GenericAbpIdentityServerWebModule>();
            Configure<AbpAutoMapperOptions>(options => { options.AddMaps<GenericAbpIdentityServerWebModule>(validate: true); });

            Configure<RazorPagesOptions>(options =>
            {
                //Configure authorization.
                options.Conventions.AuthorizePage("/IdentityServer/ApiResources/Index", IdentityServerPermissions.ApiResources.Default);
                options.Conventions.AuthorizePage("/IdentityServer/ApiResources/CreateModal", IdentityServerPermissions.ApiResources.Create);
                options.Conventions.AuthorizePage("/IdentityServer/ApiResources/EditModal", IdentityServerPermissions.ApiResources.Update);
                // options.Conventions.AuthorizePage("/PlotAreas/Index", ActivityPermissions.PlotAreas.Default);
                // options.Conventions.AuthorizePage("/PlotAreas/CreateModal", ActivityPermissions.PlotAreas.Create);
                // options.Conventions.AuthorizePage("/PlotAreas//EditModal", ActivityPermissions.PlotAreas.Update);
                // options.Conventions.AuthorizePage("/Rankings/CreateModal", ActivityPermissions.Activities.Update);
                // options.Conventions.AuthorizePage("/Rankings//EditModal", ActivityPermissions.Activities.Update);
                // options.Conventions.AuthorizePage("/Rankings//ImportModal", ActivityPermissions.Activities.Update);

            });
        }

    }
}