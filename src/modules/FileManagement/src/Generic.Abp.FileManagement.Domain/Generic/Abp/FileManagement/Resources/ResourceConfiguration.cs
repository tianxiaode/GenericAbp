using System;
using System.ComponentModel;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Resources;

public class ResourceConfiguration : AuditedEntity<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; set; }

    [DisplayName("ResourceConfiguration:AllowedFileTypes")]
    public virtual string AllowedFileTypes { get; protected set; }

    [DisplayName("ResourceConfiguration:StorageQuota")]
    public virtual long StorageQuota { get; protected set; }

    [DisplayName("ResourceConfiguration:StorageQuota")]
    public virtual long UsedStorage { get; protected set; }

    [DisplayName("ResourceConfiguration:MaxFileSize")]
    public virtual long MaxFileSize { get; protected set; }
}