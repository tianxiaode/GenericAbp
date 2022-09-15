using System.Collections.Generic;
using Volo.Abp.Identity;

namespace Generic.Abp.Identity.Roles;

public class RoleCreateOrUpdateDtoBase: IdentityRoleCreateOrUpdateDtoBase
{
    public List<string> Permissions { get; set; }
}