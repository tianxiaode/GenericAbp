using Generic.Abp.Extensions.Trees;
using Generic.Abp.FileManagement.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Folders;

public class Folder : AuditedAggregateRoot<Guid>, ITree<Folder>, IMultiTenant
{
    public virtual string Code { get; protected internal set; } = default!;
    public virtual Guid? ParentId { get; protected set; }

    [DisplayName("Folder Name")] public virtual string Name { get; protected set; } = default!;
    public Guid? TenantId { get; protected set; }
    [DisplayName("Folder:Parent")] public Folder? Parent { get; set; }
    public ICollection<Folder>? Children { get; set; }

    [DisplayName("Folder:IsInheritPermissions")]
    public virtual bool IsInheritPermissions { get; protected set; } = true;

    [DisplayName("Folder:AllowedFileTypes")]
    public virtual string AllowedFileTypes { get; protected set; } = default!;

    [DisplayName("Folder:StorageQuota")] public virtual long StorageQuota { get; protected set; } = 0;
    [DisplayName("Folder:UsedStorage")] public virtual long UsedStorage { get; protected set; } = 0;
    [DisplayName("Folder:MaxFileSize")] public virtual long MaxFileSize { get; protected set; } = 0;
    public virtual ICollection<FolderFile> Files { get; protected set; } = [];

    public Folder(Guid id, Guid? parentId, string name, Guid? tenantId = null)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        Id = id;
        ParentId = parentId;
        Name = name;
        TenantId = tenantId;
    }

    public void Rename(string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Name = name;
    }

    public void MoveTo(Guid? parentId)
    {
        ParentId = parentId;
    }

    public void SetCode(string code)
    {
        Code = code;
    }

    public void ChangeInheritPermissions(bool inherit)
    {
        IsInheritPermissions = inherit;

        AddDistributedEvent(new FolderInheritPermissionsChangeEto(Id, TenantId, inherit));
    }

    public void SetStorageQuota(long quota)
    {
        StorageQuota = quota;
    }

    public void SetUsedStorage(long usedStorage)
    {
        UsedStorage = usedStorage;
    }

    public void SetMaxFileSize(long maxFileSize) => MaxFileSize = maxFileSize;

    public void AddFile(Guid fileId)
    {
        if (IsInFiles(fileId))
        {
            return;
        }

        Files.Add(
            new FolderFile(
                Id,
                fileId,
                TenantId
            )
        );
    }

    public void RemoveFile(Guid fileId)
    {
        if (!IsInFiles(fileId))
        {
            return;
        }

        Files.RemoveAll(m => m.FileId == fileId);
    }

    public virtual bool IsInFiles(Guid fileId)
    {
        return Files.Any(m => m.FileId == fileId);
    }
}