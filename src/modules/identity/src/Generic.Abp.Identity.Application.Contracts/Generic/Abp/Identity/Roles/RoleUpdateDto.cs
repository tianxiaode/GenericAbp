using Volo.Abp.Domain.Entities;

namespace Generic.Abp.Identity.Roles;

public class RoleUpdateDto: RoleCreateOrUpdateDtoBase, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
}