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
    }

    public int TotalChunks => (int)Math.Ceiling((double)Size / ChunkSize);
}