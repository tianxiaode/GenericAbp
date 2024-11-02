using System;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.AuditLogging.AuditLogs.Dtos;

[Serializable]
public class EntityPropertyChangeDto : EntityDto<Guid>
{
    public Guid? TenantId { get; set; } = default!;

    public Guid EntityChangeId { get; set; } = default!;

    public string NewValue { get; set; } = default!;

    public string OriginalValue { get; set; } = default!;

    public string PropertyName { get; set; } = default!;

    public string PropertyTypeFullName { get; set; } = default!;
}