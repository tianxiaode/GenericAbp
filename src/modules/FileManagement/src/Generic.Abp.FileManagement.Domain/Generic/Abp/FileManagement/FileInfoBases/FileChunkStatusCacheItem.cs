using System.Collections.Generic;

namespace Generic.Abp.FileManagement.FileInfoBases;

public class FileChunkStatusCacheItem
{
    public Dictionary<int, bool> ChunkStatuses { get; set; } = default!;
}