using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.VirtualPaths.Dtos;

[Serializable]
public class VirtualPathDto : ExtensibleAuditedEntityDto<Guid>, IMultiTenant, IHasConcurrencyStamp
{
    public Guid? TenantId { get; set; } = default!;
    public string ConcurrencyStamp { get; set; } = default!;

    [DisplayName("VirtualPath:Path")] public string Path { get; set; } = default!;
}