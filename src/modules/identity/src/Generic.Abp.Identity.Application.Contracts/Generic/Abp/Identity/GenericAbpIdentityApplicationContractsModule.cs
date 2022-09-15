using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using Volo.Abp.Identity;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.Identity
{
    [DependsOn(
        typeof(AbpSettingManagementApplicationContractsModule),
        typeof(AbpIdentityApplicationContractsModule),
        typeof(GenericAbpIdentityDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class GenericAbpIdentityApplicationContractsModule : AbpModule
    {

    }
}
