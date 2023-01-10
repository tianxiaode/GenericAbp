using Generic.Abp.PhoneLogin;
using Volo.Abp.Domain;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Generic.Abp.PhoneLogin
{
    [DependsOn(
        typeof(AbpIdentityDomainModule),
        typeof(GenericAbpPhoneLoginDomainSharedModule)
    )]
    public class GenericAbpPhoneLoginDomainModule : AbpModule
    {

    }
}
