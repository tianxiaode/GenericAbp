using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Generic.Abp.Identity.Users;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IdentityUserLookupController), IncludeSelf = true)]
[RemoteService(false)]
public class AbandonIdentityUserLookupController:IdentityUserLookupController
{
    public AbandonIdentityUserLookupController(IIdentityUserLookupAppService lookupAppService) : base(lookupAppService)
    {
    }
}