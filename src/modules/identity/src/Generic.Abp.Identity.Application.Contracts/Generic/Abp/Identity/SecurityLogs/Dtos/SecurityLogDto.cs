using System;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.Identity.SecurityLogs.Dtos;

[Serializable]
public class SecurityLogDto : ExtensibleEntityDto<Guid>
{
    public Guid? TenantId { get; set; } = default!;
    public string ApplicationName { get; set; } = default!;
    public string Identity { get; set; } = default!;
    public string Action { get; set; } = default!;
    public Guid? UserId { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string TenantName { get; set; } = default!;
    public string ClientId { get; set; } = default!;
    public string CorrelationId { get; set; } = default!;
    public string ClientIpAddress { get; set; } = default!;
    public string BrowserInfo { get; set; } = default!;
    public DateTime CreationTime { get; set; } = default!;
}