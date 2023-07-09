using Generic.Abp.Enumeration;
using Generic.Abp.ExtResource;
using Generic.Abp.Identity;
using Generic.Abp.OpenIddict;
using Generic.Abp.PhoneLogin;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.Demo
{
    [DependsOn(
        typeof(DemoDomainModule),
        typeof(AbpAccountApplicationModule),
        typeof(DemoApplicationContractsModule),
        //typeof(AbpIdentityApplicationModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpTenantManagementApplicationModule),
        typeof(AbpFeatureManagementApplicationModule),
        typeof(AbpSettingManagementApplicationModule),
        typeof(GenericAbpEnumerationApplicationModule),
        typeof(GenericAbpExtResourceApplicationModule),
        typeof(GenericAbpIdentityApplicationModule),
        typeof(GenericAbpOpenIddictApplicationModule),
        typeof(GenericAbpPhoneLoginApplicationModule)
    )]
    public class DemoApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options => { options.AddMaps<DemoApplicationModule>(); });
        }
    }
}