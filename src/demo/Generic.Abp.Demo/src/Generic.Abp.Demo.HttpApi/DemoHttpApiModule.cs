using Generic.Abp.Application;
using Generic.Abp.Demo.Localization;
using Generic.Abp.Enumeration;
using Generic.Abp.ExtResource;
using Generic.Abp.Identity;
using Generic.Abp.IdentityServer;
using Localization.Resources.AbpUi;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.TenantManagement;

namespace Generic.Abp.Demo
{
    [DependsOn(
        typeof(DemoApplicationContractsModule),
        typeof(AbpAccountHttpApiModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpTenantManagementHttpApiModule),
        typeof(AbpFeatureManagementHttpApiModule),
        typeof(GenericAbpExtResourceHttpApiModule),
        typeof(GenericAbpIdentityHttpApiModule),
        typeof(GenericAbpIdentityServerHttpApiModule),
        typeof(GenericAbpApplicationModule),
        typeof(GenericAbpEnumerationHttpApiModule)
        )]
    public class DemoHttpApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureLocalization();
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<DemoResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );
            });
        }

    }
}
