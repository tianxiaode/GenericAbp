using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Dtos;

[Serializable]
public class PermissionDto : ExtensibleAuditedEntityDto<Guid>, IMultiTenant, IHasConcurrencyStamp
{
    public Guid? TenantId { get; set; } = default!;
    public string ConcurrencyStamp { get; set; } = default!;

    [DisplayName("Permission:ProviderName")]
    public string ProviderName { get; set; } = default!;

    [DisplayName("Permission:ProviderKey")]
    public string? ProviderKey { get; set; } = default!;

    [DisplayName("Permission:CanRead")] public bool CanRead { get; set; }
    [DisplayName("Permission:CanWrite")] public bool CanWrite { get; set; }
    [DisplayName("Permission:CanDelete")] public bool CanDelete { get; set; }
}