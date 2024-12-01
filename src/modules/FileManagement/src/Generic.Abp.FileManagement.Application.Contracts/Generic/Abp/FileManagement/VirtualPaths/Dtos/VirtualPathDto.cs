using System;
using Generic.Abp.FileManagement.Resources.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.VirtualPaths.Dtos;

[Serializable]
public class VirtualPathDto : ExtensibleAuditedEntityDto<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public ResourceBaseDto Resource { get; set; } = default!;
    public Guid ResourceId { get; set; } = default!;
    public bool IsAccessible { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}