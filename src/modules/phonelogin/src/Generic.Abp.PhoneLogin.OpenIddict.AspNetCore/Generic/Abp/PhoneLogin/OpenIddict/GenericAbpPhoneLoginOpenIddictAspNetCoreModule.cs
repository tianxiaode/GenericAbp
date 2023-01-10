using Generic.Abp.PhoneLogin;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;

namespace Generic.Abp.PhoneLogin.OpenIddict
{
    [DependsOn(
        typeof(AbpOpenIddictAspNetCoreModule),
        typeof(GenericAbpPhoneLoginDomainModule)
    )]

    public class GenericAbpPhoneLoginOpenIddictAspNetCoreModule : AbpModule
    {
    }
}
