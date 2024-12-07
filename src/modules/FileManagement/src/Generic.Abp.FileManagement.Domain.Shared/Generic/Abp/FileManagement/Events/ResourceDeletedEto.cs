using System;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Events;

[Serializable]
public class ResourceDeletedEto : IMultiTenant
{
    public Guid ResourceId { get; set; } = default!;
    public Guid? TenantId { get; set; } = default!;
}