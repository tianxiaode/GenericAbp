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
                options.Conventions.AuthorizePage("/IdentityServer/ApiResources/CreateApiResourcePropertyModal", IdentityServerPermissions.ApiResources.Update);
                options.Conventions.AuthorizePage("/IdentityServer/ApiResources/CreateApiResourceSecretModal", IdentityServerPermissions.ApiResources.Update);

                options.Conventions.AuthorizePage("/IdentityServer/ApiScopes/CreateApiScopePropertyModal", IdentityServerPermissions.ApiResources.Update);
                options.Conventions.AuthorizePage("/IdentityServer/ApiScopes/CreateModal", IdentityServerPermissions.ApiResources.Update);
                options.Conventions.AuthorizePage("/IdentityServer/ApiScopes/EditModal", IdentityServerPermissions.ApiResources.Update);

                options.Conventions.AuthorizePage("/IdentityServer/Clients/Index", IdentityServerPermissions.Clients.Default);
                options.Conventions.AuthorizePage("/IdentityServer/Clients/CreateModal", IdentityServerPermissions.Clients.Create);
                options.Conventions.AuthorizePage("/IdentityServer/Clients/EditModal", IdentityServerPermissions.Clients.Update);
                options.Conventions.AuthorizePage("/IdentityServer/Clients/CreateClientCorsOriginModal", IdentityServerPermissions.Clients.Update);
                options.Conventions.AuthorizePage("/IdentityServer/Clients/CreateClientGrantTypeModal", IdentityServerPermissions.Clients.Update);
                options.Conventions.AuthorizePage("/IdentityServer/Clients/CreateClientIdentityProviderRestrictionModal", IdentityServerPermissions.Clients.Update);
                options.Conventions.AuthorizePage("/IdentityServer/Clients/CreateClientPostLogoutRedirectUriModal", IdentityServerPermissions.Clients.Update);
                options.Conventions.AuthorizePage("/IdentityServer/Clients/CreateClientRedirectUriModal", IdentityServerPermissions.Clients.Update);
                options.Conventions.AuthorizePage("/IdentityServer/Clients/CreateClientPropertyModalModel", IdentityServerPermissions.Clients.Update);
                options.Conventions.AuthorizePage("/IdentityServer/Clients/CreateClientScopeModal", IdentityServerPermissions.Clients.Update);
                options.Conventions.AuthorizePage("/IdentityServer/Clients/CreateClientSecretModal", IdentityServerPermissions.Clients.Update);

                options.Conventions.AuthorizePage("/IdentityServer/IdentityResources/Index", IdentityServerPermissions.IdentityResources.Default);
                options.Conventions.AuthorizePage("/IdentityServer/IdentityResources/CreateModal", IdentityServerPermissions.IdentityResources.Create);
                options.Conventions.AuthorizePage("/IdentityServer/IdentityResources/EditModal", IdentityServerPermissions.IdentityResources.Update);
                options.Conventions.AuthorizePage("/IdentityServer/IdentityResources/CreateIdentityResourcePropertyModal", IdentityServerPermissions.IdentityResources.Update);

            });
        }

    }
}