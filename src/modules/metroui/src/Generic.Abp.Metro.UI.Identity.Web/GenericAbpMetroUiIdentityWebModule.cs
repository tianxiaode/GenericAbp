using Generic.Abp.Metro.UI.Identity.Web.Navigation;
using Generic.Abp.Metro.UI.Theme.Shared;
using Generic.Abp.W2Ui;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AutoMapper;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using CreateModalModel = Generic.Abp.Metro.UI.Identity.Web.Pages.Identity.Roles.CreateModalModel;
using EditModalModel = Generic.Abp.Metro.UI.Identity.Web.Pages.Identity.Roles.EditModalModel;

namespace Generic.Abp.Metro.UI.Identity.Web;

[DependsOn(typeof(AbpIdentityApplicationContractsModule))]
[DependsOn(typeof(AbpAutoMapperModule))]
[DependsOn(typeof(GenericAbpMetroUiThemeSharedModule))]
[DependsOn(typeof(GenericAbpW2UiModule))]
public class GenericAbpMetroUiIdentityWebModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(IdentityResource),
                typeof(GenericAbpMetroUiIdentityWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpMetroUiIdentityWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AbpIdentityWebMainMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GenericAbpMetroUiIdentityWebModule>();
        });

        context.Services.AddAutoMapperObjectMapper<GenericAbpMetroUiIdentityWebModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<GenericAbpMetroUiIdentityWebAutoMapperProfile>(validate: true);
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Identity/Users/Index", IdentityPermissions.Users.Default);
            options.Conventions.AuthorizePage("/Identity/Users/CreateModal", IdentityPermissions.Users.Create);
            options.Conventions.AuthorizePage("/Identity/Users/EditModal", IdentityPermissions.Users.Update);
            options.Conventions.AuthorizePage("/Identity/Roles/Index", IdentityPermissions.Roles.Default);
            options.Conventions.AuthorizePage("/Identity/Roles/CreateModal", IdentityPermissions.Roles.Create);
            options.Conventions.AuthorizePage("/Identity/Roles/EditModal", IdentityPermissions.Roles.Update);
        });


        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(IdentityRemoteServiceConsts.ModuleName);
        });
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToUi(
                    IdentityModuleExtensionConsts.ModuleName,
                    IdentityModuleExtensionConsts.EntityNames.Role,
                    createFormTypes: new[]
                        { typeof(CreateModalModel.RoleInfoModel) },
                    editFormTypes: new[]
                        { typeof(EditModalModel.RoleInfoModel) }
                );

            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToUi(
                    IdentityModuleExtensionConsts.ModuleName,
                    IdentityModuleExtensionConsts.EntityNames.User,
                    createFormTypes: new[]
                        { typeof(Pages.Identity.Users.CreateModalModel.UserInfoViewModel) },
                    editFormTypes: new[]
                        { typeof(Pages.Identity.Users.EditModalModel.UserInfoViewModel) }
                );
        });
    }
}