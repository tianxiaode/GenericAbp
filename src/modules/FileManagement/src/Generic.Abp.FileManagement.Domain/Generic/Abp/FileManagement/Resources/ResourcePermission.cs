using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Generic.Abp.FileManagement.Resources;

public class ResourcePermission : AuditedEntity<Guid>, IResourcePermission
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual Guid ResourceId { get; protected set; }
    public virtual string ProviderName { get; protected set; }
    public virtual string? ProviderKey { get; protected set; }
    public virtual bool CanRead { get; protected set; }
    public virtual bool CanWrite { get; protected set; }
    public virtual bool CanDelete { get; protected set; }

    public ResourcePermission(Guid id, Guid resourceId, string providerName, string? providerKey, bool canRead = false,
        bool canWrite = false, bool canDelete = false, Guid? tenantId = null) : base(id)
    {
        Check.NotNullOrEmpty(providerName, nameof(providerName));
        ResourceId = resourceId;
        ProviderName = providerName;
        ProviderKey = providerKey;
        CanRead = canRead;
        CanWrite = canWrite;
        CanDelete = canDelete;
        TenantId = tenantId;
    }
}