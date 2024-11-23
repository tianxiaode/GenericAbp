using System;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Events;

[Serializable]
public class ResourceDeletedEto : IMultiTenant
{
    public Guid ResourceId { get; set; }
    public Guid? FileInfoBaseId { get; set; }
    public Guid? TenantId { get; set; }
}