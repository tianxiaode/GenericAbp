using System;
using System.ComponentModel;
using Generic.Abp.FileManagement.Files;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.FileInfoBases;

public class FileInfoBase : AuditedEntity<Guid>, IMultiTenant, IFileInfoBase
{
    public virtual Guid? TenantId { get; protected set; }
    [DisplayName("File:MimeType")] public virtual string MimeType { get; protected set; }
    [DisplayName("File:FileType")] public virtual string FileType { get; protected set; }
    [DisplayName("File:Size")] public virtual long Size { get; protected set; }
    [DisplayName("File:Hash")] public virtual string Hash { get; protected set; }
    [DisplayName("File:Path")] public virtual string Path { get; protected set; }

    public FileInfoBase(Guid id, string hash, string mimeType, string fileType, long size, string path,
        Guid? tenantId = null) : base(id)
    {
        Check.NotNullOrEmpty(hash, nameof(Hash));
        Check.NotNullOrEmpty(mimeType, nameof(MimeType));
        Check.NotNullOrEmpty(fileType, nameof(FileType));
        Check.NotNullOrEmpty(path, nameof(Path));

        TenantId = tenantId;
        MimeType = mimeType;
        FileType = fileType;
        Size = size;
        Hash = hash;
        Path = path;
    }
}