using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;

namespace Generic.Abp.AuditLogging.AuditLogs.Dtos;

[Serializable]
public class GetEntityChangesListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; } = default!;
    public DateTime? StartTime { get; set; } = default!;
    public DateTime? EndTime { get; set; } = default!;
    public EntityChangeType? ChangeType { get; set; } = default!;
}