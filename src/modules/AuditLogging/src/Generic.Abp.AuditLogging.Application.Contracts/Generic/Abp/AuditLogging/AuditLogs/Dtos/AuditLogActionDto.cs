using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace Generic.Abp.AuditLogging.AuditLogs.Dtos;

[Serializable]
public class AuditLogActionDto : ExtensibleEntityDto<Guid>
{
    public Guid? TenantId { get; set; } = default!;

    public Guid AuditLogId { get; set; } = default!;

    public string ServiceName { get; set; } = default!;

    public string MethodName { get; set; } = default!;

    public string Parameters { get; set; } = default!;

    public DateTime ExecutionTime { get; set; } = default!;

    public int ExecutionDuration { get; set; } = default!;
}