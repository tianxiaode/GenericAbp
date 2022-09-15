using Generic.Abp.Identity.Roles;

namespace Generic.Abp.Identity.Users;

public class UserGetRoleDto: RoleDto
{
    public bool IsSelected { get; set; }

}