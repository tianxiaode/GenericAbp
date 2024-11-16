using System;
using System.ComponentModel;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Files;

public class File : AuditedAggregateRoot<Guid>, IFile, IMultiTenant
{
    public Guid? TenantId { get; protected set; }

    [DisplayName("File:Filename")] public virtual string Filename { get; protected set; } = default!;
    [DisplayName("File:MimeType")] public virtual string MimeType { get; protected set; }
    [DisplayName("File:FileType")] public virtual string FileType { get; protected set; }
    [DisplayName("File:Size")] public virtual long Size { get; protected set; } = 0;
    [DisplayName("File:Description")] public virtual string Description { get; protected set; } = default!;
    [DisplayName("File:Hash")] public virtual string Hash { get; protected set; }
    [DisplayName("File:Path")] public virtual string Path { get; protected set; } = default!;

    [DisplayName("File:IsInheritPermissions")]
    public virtual bool IsInheritPermissions { get; protected set; } = true;

    public virtual Guid FolderId { get; protected set; }

    public File(Guid id, Guid folderId, string hash, string mimeType, string fileType, long size,
        Guid? tenantId = null) : base(id)
    {
        Check.NotNullOrEmpty(hash, nameof(Hash));
        Check.NotNullOrEmpty(mimeType, nameof(MimeType));
        Check.NotNullOrEmpty(fileType, nameof(FileType));

        FolderId = folderId;
        TenantId = tenantId;
        MimeType = mimeType;
        FileType = fileType;
        Size = size;
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

    public void SetPath(string path)
    {
        Check.NotNullOrEmpty(path, nameof(Path));
        Path = path;
    }
}