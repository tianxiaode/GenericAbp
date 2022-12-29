using Generic.Abp.Enumeration;
using Generic.Abp.ExtResource;
using Generic.Abp.Identity;
using Generic.Abp.IdentityServer;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;

namespace Generic.Abp.Demo
{
    [DependsOn(
        typeof(DemoDomainSharedModule),
        typeof(AbpAccountApplicationContractsModule),
        typeof(AbpFeatureManagementApplicationContractsModule),
        typeof(AbpIdentityApplicationContractsModule),
        typeof(AbpPermissionManagementApplicationContractsModule),
        typeof(AbpTenantManagementApplicationContractsModule),
        typeof(AbpObjectExtendingModule),
        typeof(GenericAbpEnumerationDomainSharedModule),
        typeof(GenericAbpExtResourceApplicationContractsModule),
        typeof(GenericAbpIdentityApplicationContractsModule),
        typeof(GenericAbpIdentityServerApplicationContractsModule),
        typeof(GenericAbpEnumerationApplicationContractsModule)
    )]
    public class DemoApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            DemoDtoExtensions.Configure();
        }
    }
}
