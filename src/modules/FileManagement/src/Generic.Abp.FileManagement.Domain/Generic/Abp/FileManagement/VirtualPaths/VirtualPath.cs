using System;
using System.ComponentModel;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.VirtualPaths;

public class VirtualPath : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual Guid FolderId { get; protected set; }
    [DisplayName("VirtualPath:Path")] public virtual string Path { get; protected set; }

    public VirtualPath(Guid id, Guid folderId, string path, Guid? tenantId = null) : base(id)
    {
        Check.NotNull(path, nameof(path));
        FolderId = folderId;
        Path = path;
        TenantId = tenantId;
    }

    public virtual void ChangeVirtualPath(string virtualPath)
    {
        Check.NotNull(virtualPath, nameof(virtualPath));
        Path = virtualPath;
    }
}