using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Generic.Abp.PhoneLogin
{
    [DependsOn(
        typeof(GenericAbpPhoneLoginDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class GenericAbpPhoneLoginApplicationContractsModule : AbpModule
    {

    }
}
