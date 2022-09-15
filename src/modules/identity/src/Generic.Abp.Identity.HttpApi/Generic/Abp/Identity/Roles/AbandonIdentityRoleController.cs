using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Generic.Abp.Identity.Roles;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IdentityRoleController), IncludeSelf = true)]
[RemoteService(false)]

public class AbandonIdentityRoleController: IdentityRoleController
{
    public AbandonIdentityRoleController(IIdentityRoleAppService roleAppService) : base(roleAppService)
    {
    }

}