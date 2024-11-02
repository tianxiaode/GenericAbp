using System;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.AuditLogging.AuditLogs.Dtos;

[Serializable]
public class AuditLogGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; } = default!;
    public DateTime? StartTime { get; set; } = default!;
    public DateTime? EndTime { get; set; } = default!;
    public string? ApplicationName { get; set; } = default!;
    public string? UserName { get; set; } = default!;
    public string? Url { get; set; } = default!;
    public string? ClientId { get; set; } = default!;
    public string? CorrelationId { get; set; } = default!;
    public int? MinExecutionDuration { get; set; } = default!;
    public int? MaxExecutionDuration { get; set; } = default!;
}