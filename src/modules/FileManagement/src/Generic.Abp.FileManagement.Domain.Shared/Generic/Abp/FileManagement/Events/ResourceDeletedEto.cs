using System;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Events;

[Serializable]
public class ResourceDeletedEto : IMultiTenant
{
    public List<Guid> ResourceIds { get; set; } = default!;
    public Guid ParentId { get; set; } = default!;
    public Guid? TenantId { get; set; } = default!;
}