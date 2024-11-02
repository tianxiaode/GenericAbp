using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.AuditLogging.AuditLogs.Dtos;

[Serializable]
public class AuditLogDto : ExtensibleEntityDto<Guid>, IHasConcurrencyStamp
{
    public string ApplicationName { get; set; } = default!;

    public Guid? UserId { get; set; } = default!;

    public string UserName { get; set; } = default!;

    public Guid? TenantId { get; set; } = default!;

    public string TenantName { get; set; } = default!;

    public Guid? ImpersonatorUserId { get; set; } = default!;

    public string ImpersonatorUserName { get; set; } = default!;

    public Guid? ImpersonatorTenantId { get; set; } = default!;

    public string ImpersonatorTenantName { get; set; } = default!;

    public DateTime ExecutionTime { get; set; } = default!;

    public int ExecutionDuration { get; set; } = default!;

    public string ClientIpAddress { get; set; } = default!;

    public string ClientName { get; set; } = default!;

    public string ClientId { get; set; } = default!;

    public string CorrelationId { get; set; } = default!;

    public string BrowserInfo { get; set; } = default!;

    public string HttpMethod { get; set; } = default!;

    public string Url { get; set; } = default!;

    public string Exceptions { get; set; } = default!;

    public string Comments { get; set; } = default!;

    public int? HttpStatusCode { get; set; } = default!;

    //public ICollection<EntityChangeDto> EntityChanges { get; set; } = default!;

    public ICollection<AuditLogActionDto> Actions { get; set; } = default!;

    public string ConcurrencyStamp { get; set; } = default!;
}