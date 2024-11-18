using System;
using System.ComponentModel;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.FileInfoBases;

public class FileInfoBase : AuditedEntity<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    [DisplayName("File:MimeType")] public virtual string MimeType { get; protected set; }
    [DisplayName("File:FileType")] public virtual string FileType { get; protected set; }
    [DisplayName("File:Size")] public virtual long Size { get; protected set; } = 0;
    [DisplayName("File:Hash")] public virtual string Hash { get; protected set; }
    [DisplayName("File:Path")] public virtual string Path { get; protected set; } = default!;

    public FileInfoBase(Guid id, string hash, string mimeType, string fileType, long size,
        Guid? tenantId = null) : base(id)
    {
        Check.NotNullOrEmpty(hash, nameof(Hash));
        Check.NotNullOrEmpty(mimeType, nameof(MimeType));
        Check.NotNullOrEmpty(fileType, nameof(FileType));

        TenantId = tenantId;
        MimeType = mimeType;
        FileType = fileType;
        Size = size;
        Hash = hash;
    }

    public void SetPath(string path)
    {
        Check.NotNullOrEmpty(path, nameof(Path));
        Path = path;
    }
}