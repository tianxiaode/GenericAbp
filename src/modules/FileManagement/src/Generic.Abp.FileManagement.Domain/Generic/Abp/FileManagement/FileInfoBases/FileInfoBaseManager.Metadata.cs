using Microsoft.Extensions.Caching.Distributed;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Generic.Abp.FileManagement.FileInfoBases;

public partial class FileInfoBaseManager
{
    public virtual async Task<FileMetadataCacheItem> GetFileMetadataCacheAsync(string hash, long size, long chunkSize,
        string filename, string mimeType, string extension)
    {
        var cacheItemName = await GetFileMetadataCacheNameAsync(hash);
        var cacheItem = await FileMetadataCache.GetOrAddAsync(
            cacheItemName,
            async () => await GetOrCreateMetadataAsync(hash, size, chunkSize, filename, mimeType, extension),
            () => new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddHours(5)
            },
            token: CancellationToken
        );
        if (cacheItem == null)
        {
            throw new Exception($"Cache item {cacheItemName} is null");
        }

        return cacheItem;
    }

    public virtual async Task<FileMetadataCacheItem> GetOrCreateMetadataAsync(string hash, long size, long chunkSize,
        string filename, string mimeType, string extension)
    {
        try
        {
            var dir = await GetAndCheckTempPathAsync(hash);

            var metadataPath = Path.Combine(dir, await GetMetadataFileNameAsync(hash));

            await using var stream = new FileStream(metadataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var metadata = await JsonSerializer.DeserializeAsync<FileMetadataCacheItem>(stream);
            if (metadata != null)
            {
                return metadata;
            }

            metadata = new FileMetadataCacheItem(hash, filename, size, chunkSize, filename, extension);
            await JsonSerializer.SerializeAsync(stream, metadata);

            return metadata;
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while creating metadata for hash: {Hash}", hash);
            throw;
        }
    }

    protected virtual Task<string> GetMetadataFileNameAsync(string hash)
    {
        return Task.FromResult($"{hash}_metadata.json");
    }

    protected virtual Task<string> GetFileMetadataCacheNameAsync(string hash)
    {
        return Task.FromResult($"FileManagement_FileInfoBaseManager_file_metadata_cache_{hash}");
    }
}