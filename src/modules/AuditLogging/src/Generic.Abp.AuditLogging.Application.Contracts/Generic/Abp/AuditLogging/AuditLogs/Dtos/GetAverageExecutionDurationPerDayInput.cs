using System;

namespace Generic.Abp.AuditLogging.AuditLogs.Dtos;

[Serializable]
public class GetAverageExecutionDurationPerDayInput
{
    public DateTime StartTime { get; set; } = default!;
    public DateTime EndTime { get; set; } = default!;
}