using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Data;

namespace Generic.Abp.AuditLogging.AuditLogs.Dtos;

[Serializable]
public class EntityChangeDto : ExtensibleEntityDto<Guid>
{
    public Guid AuditLogId { get; set; } = default!;

    public Guid? TenantId { get; set; } = default!;

    public DateTime ChangeTime { get; set; } = default!;

    public EntityChangeType ChangeType { get; set; } = default!;

    public Guid? EntityTenantId { get; set; } = default!;

    public string EntityId { get; set; } = default!;

    public string EntityTypeFullName { get; set; } = default!;

    public ICollection<EntityPropertyChangeDto> PropertyChanges { get; set; } = default!;
}