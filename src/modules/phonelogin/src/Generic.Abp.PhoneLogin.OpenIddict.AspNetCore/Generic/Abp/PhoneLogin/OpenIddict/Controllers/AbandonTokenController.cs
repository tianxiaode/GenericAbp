using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.PhoneLogin.OpenIddict.Controllers
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(Volo.Abp.OpenIddict.Controllers.TokenController), IncludeSelf = true)]
    [RemoteService(false)]
    public class AbandonTokenController : Volo.Abp.OpenIddict.Controllers.TokenController
    {
    }
}
