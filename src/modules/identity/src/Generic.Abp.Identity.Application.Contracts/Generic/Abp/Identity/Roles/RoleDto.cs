using System.Collections.Generic;
using Volo.Abp.Identity;

namespace Generic.Abp.Identity.Roles;

public class RoleDto: IdentityRoleDto
{
    public List<string> Permissions { get; set; }
    public List<RoleTranslationDto> Translations { get; set; }


}