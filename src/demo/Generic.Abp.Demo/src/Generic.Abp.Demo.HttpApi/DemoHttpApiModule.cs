﻿using Generic.Abp.Application;
using Generic.Abp.Demo.Localization;
using Generic.Abp.Enumeration;
using Generic.Abp.ExtResource;
using Generic.Abp.Identity;
using Generic.Abp.MenuManagement;
using Generic.Abp.OpenIddict;
using Localization.Resources.AbpUi;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace Generic.Abp.Demo
{
    [DependsOn(
        typeof(DemoApplicationContractsModule),
        typeof(AbpAccountHttpApiModule),
        //typeof(AbpIdentityHttpApiModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpTenantManagementHttpApiModule),
        typeof(AbpFeatureManagementHttpApiModule),
        typeof(AbpSettingManagementHttpApiModule),
        typeof(GenericAbpIdentityHttpApiModule),
        typeof(GenericAbpApplicationModule),
        typeof(GenericAbpEnumerationHttpApiModule),
        typeof(GenericAbpOpenIddictHttpApiModule),
        typeof(GenericAbpMenuManagementHttpApiModule)
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