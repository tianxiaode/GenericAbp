using System;
using System.Collections.Generic;

namespace Generic.Abp.FileManagement.FileInfoBases;

[Serializable]
public class FileMetadataCacheItem(string hash, long size, long chunkSize, string fileName)
{
    public string Hash { get; set; } = hash;
    public string? MimeType { get; set; } = default!;
    public long Size { get; set; } = size;
    public long ChunkSize { get; set; } = chunkSize;
    public string FileName { get; set; } = fileName;
    public HashSet<int> UploadedChunks = default!;

    public int TotalChunks => (int)Math.Ceiling((double)Size / ChunkSize);
}