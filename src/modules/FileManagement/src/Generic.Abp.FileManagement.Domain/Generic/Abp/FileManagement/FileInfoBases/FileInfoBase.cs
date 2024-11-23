using System;
using System.ComponentModel;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Generic.Abp.FileManagement.FileInfoBases;

public class FileInfoBase : AuditedEntity<Guid>, IFileInfoBase
{
    public virtual Guid? TenantId { get; protected set; }
    [DisplayName("File:MimeType")] public virtual string MimeType { get; protected set; }
    [DisplayName("File:Extension")] public virtual string Extension { get; protected set; }
    [DisplayName("File:Size")] public virtual long Size { get; protected set; }
    [DisplayName("File:Hash")] public virtual string Hash { get; protected set; }
    [DisplayName("File:Path")] public virtual string Path { get; protected set; }

    [DisplayName("File:RetentionPolicy")] public virtual FileRetentionPolicy RetentionPolicy { get; protected set; }

    [DisplayName("File:ExpireAt")] public virtual DateTime? ExpireAt { get; protected set; }

    public FileInfoBase(Guid id, string hash, string mimeType, string fileType, long size, string path,
        Guid? tenantId = null) : base(id)
    {
        Check.NotNullOrEmpty(hash, nameof(Hash));
        Check.NotNullOrEmpty(mimeType, nameof(MimeType));
        Check.NotNullOrEmpty(fileType, nameof(Extension));
        Check.NotNullOrEmpty(path, nameof(Path));

        TenantId = tenantId;
        MimeType = mimeType;
        Extension = fileType;
        Size = size;
        Hash = hash;
        Path = path;
        RetentionPolicy = FileRetentionPolicy.Default;
    }

    public void SetExpireAt(DateTime expireAt)
    {
        ExpireAt = expireAt;
    }

    public void SetRetentionPolicy(FileRetentionPolicy retentionPolicy)
    {
        RetentionPolicy = retentionPolicy;
    }
}