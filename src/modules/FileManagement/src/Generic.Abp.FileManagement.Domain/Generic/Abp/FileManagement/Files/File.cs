using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Generic.Abp.FileManagement.Files;

public class File: AuditedAggregateRoot<Guid>, IFile
{
    public string Filename { get; protected set; }
    public string MimeType { get; protected set; }
    public string FileType { get; protected set; }
    public long Size { get; protected set; }
    public string Description { get; protected set; }
    public string Hash { get; protected set; }
    public string Path { get; protected set; }

    public File(Guid id, string hash, string mimeType, string fileType, long size) : base(id)
    {
        Check.NotNullOrEmpty(hash, nameof(Hash));
        Check.NotNullOrEmpty(mimeType, nameof(MimeType));
        Check.NotNullOrEmpty(fileType, nameof(FileType));

        MimeType = mimeType;
        FileType = fileType;
        Size = size;
        Hash = hash;
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