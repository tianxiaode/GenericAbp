using Generic.Abp.Domain;
using Volo.Abp.Domain;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.Identity
{
    [DependsOn(
        typeof(GenericAbpDddDomainModule),
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpIdentityDomainModule),
        typeof(GenericAbpIdentityDomainSharedModule)
    )]
    public class GenericAbpIdentityDomainModule : AbpModule
    {

    }
}
