using Volo.Abp.Account.Web;
using Volo.Abp.Modularity;

namespace Generic.Abp.PhoneLogin.Web.Account
{

    [DependsOn(
        typeof(AbpAccountWebModule),
        typeof(GenericAbpPhoneLoginDomainModule)
    )]
    public class GenericAbpPhoneLoginAccountWebModule : AbpModule
    {

    }
}