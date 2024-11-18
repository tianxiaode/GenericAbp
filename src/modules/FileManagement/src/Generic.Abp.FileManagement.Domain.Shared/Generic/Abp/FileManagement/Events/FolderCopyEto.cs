using System;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Events;

[Serializable]
public class FolderCopyEto(Guid sourceId, Guid destinationId, Guid? tenantId) : IMultiTenant
{
    public Guid SourceId { get; set; } = sourceId;
    public Guid DestinationId { get; set; } = destinationId;
    public Guid? TenantId { get; set; } = tenantId;
}