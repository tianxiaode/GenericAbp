using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;

namespace Generic.Abp.PhoneLogin.IdentityServer
{
    [DependsOn(
        typeof(AbpIdentityServerDomainModule),
        typeof(GenericAbpPhoneLoginDomainModule)
    )]
    public class GenericAbpPhoneLoginIdentityServerModule : AbpModule
    {
    }
}
