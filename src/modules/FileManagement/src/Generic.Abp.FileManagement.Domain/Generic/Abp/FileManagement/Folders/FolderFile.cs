using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Folders;

public class FolderFile : CreationAuditedEntity<Guid>, IMultiTenant
{
    public Guid FolderId { get; protected set; }
    public Guid FileId { get; protected set; }
    public Guid? TenantId { get; protected set; }

    public FolderFile(Guid id, Guid folderId, Guid fileId, Guid? tenantId = null)
    {
        FolderId = folderId;
        FileId = fileId;
        TenantId = tenantId;
    }

    public override object?[] GetKeys()
    {
        return [FolderId, FileId];
    }
}