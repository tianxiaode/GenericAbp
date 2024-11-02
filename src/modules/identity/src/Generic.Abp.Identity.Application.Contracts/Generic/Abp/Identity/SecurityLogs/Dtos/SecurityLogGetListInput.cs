using System;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.Identity.SecurityLogs.Dtos;

[Serializable]
public class SecurityLogGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; } = default!;
    public DateTime? StartTime { get; set; } = default!;
    public DateTime? EndTime { get; set; } = default!;
    public string? ApplicationName { get; set; } = default!;
    public string? Identity { get; set; } = default!;
    public string? ActionName { get; set; } = default!;
    public string? UserName { get; set; } = default!;
    public string? ClientId { get; set; } = default!;
    public string? CorrelationId { get; set; } = default!;
}