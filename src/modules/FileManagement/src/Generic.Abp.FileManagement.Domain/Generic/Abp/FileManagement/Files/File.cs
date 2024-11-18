using System;
using System.ComponentModel;
using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Folders;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Files;

public class File : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    [DisplayName("File:Filename")] public virtual string Filename { get; protected set; } = default!;
    [DisplayName("File:Description")] public virtual string Description { get; protected set; } = default!;
    [DisplayName("File:Hash")] public virtual string Hash { get; protected set; }

    [DisplayName("File:IsInheritPermissions")]
    public virtual bool IsInheritPermissions { get; protected set; } = true;

    public virtual Guid FolderId { get; protected set; }
    public virtual Folder Folder { get; set; } = default!;
    public virtual Guid FileInfoBaseId { get; set; }
    public virtual FileInfoBase FileInfoBase { get; set; } = default!;

    public File(Guid id, Guid folderId, string hash, Guid? tenantId = null) : base(id)
    {
        Check.NotNullOrEmpty(hash, nameof(Hash));

        FolderId = folderId;
        TenantId = tenantId;
        Hash = hash;
        IsInheritPermissions = true;
    }

    public void SetFilename(string filename)
    {
        Check.NotNullOrEmpty(filename, nameof(Filename));
        Filename = filename;
    }

    public void SetDescription(string description)
    {
        Check.NotNullOrEmpty(description, nameof(Description));
        Description = description;
    }

    public void SetFileInfoBase(FileInfoBase fileInfoBase)
    {
        FileInfoBaseId = fileInfoBase.Id;
    }

    public void ChangeInheritPermissions(bool inheritPermissions)
    {
        IsInheritPermissions = inheritPermissions;
    }

    public void ChangeFolder(Folder folder)
    {
        FolderId = folder.Id;
    }
}