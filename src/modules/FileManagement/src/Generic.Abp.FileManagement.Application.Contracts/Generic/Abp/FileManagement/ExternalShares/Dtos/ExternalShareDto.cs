using Generic.Abp.FileManagement.Resources.Dtos;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.ExternalShares.Dtos;

[Serializable]
public class ExternalShareDto : ExtensibleAuditedEntityDto<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string LinkName { get; set; } = default!;
    public ResourceBaseDto Resource { get; set; } = default!;
    public Guid ResourceId { get; set; } = default!;
    public string Password { get; set; } = default!;
    public DateTime ExpireTime { get; set; } = default!;
}