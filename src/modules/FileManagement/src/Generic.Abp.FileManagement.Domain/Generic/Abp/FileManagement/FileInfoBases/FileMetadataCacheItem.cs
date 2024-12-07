using System;

namespace Generic.Abp.FileManagement.FileInfoBases;

[Serializable]
public class FileMetadataCacheItem
{
    public string Hash { get; set; } = default!;
    public string Filename { get; set; } = default!;
    public long Size { get; set; } = 0;
    public long ChunkSize { get; set; } = 0;
    public string Mimetype { get; set; } = default!;
    public string Extension { get; set; } = default!;
    public Guid FolderId { get; set; } = default!;
    public DateTime UploadStartTime { get; set; } = default!;

    /// <summary>
    /// 用于自动清理临时文件夹时判断是否过期，以便给大文件留足够续传时间
    /// </summary>
    public DateTimeOffset ExpirationTime { get; set; } = default!;

    public FileMetadataCacheItem()
    {
    }

    public FileMetadataCacheItem(string hash, string filename, long size, long chunkSize, string mimetype,
        string extension)
    {
        Hash = hash;
        Filename = filename;
        Size = size;
        ChunkSize = chunkSize;
        Mimetype = mimetype;
        Extension = extension;
        UploadStartTime = DateTime.UtcNow;
    }

    public int TotalChunks => (int)Math.Ceiling((double)Size / ChunkSize);
}