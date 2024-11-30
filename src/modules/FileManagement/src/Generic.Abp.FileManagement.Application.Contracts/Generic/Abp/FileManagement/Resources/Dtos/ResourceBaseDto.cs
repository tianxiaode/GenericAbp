using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Resources.Dtos;

[Serializable]
public class ResourceBaseDto : ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string ConcurrencyStamp { get; set; } = default!;
    public ResourceType Type { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public Guid? OwnerId { get; set; } = default!;
    public bool IsStatic { get; set; } = false;
    public bool IsEnabled { get; set; } = false;
}