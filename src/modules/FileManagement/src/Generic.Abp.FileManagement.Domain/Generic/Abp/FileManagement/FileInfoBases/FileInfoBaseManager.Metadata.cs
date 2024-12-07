using Microsoft.Extensions.Caching.Distributed;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.Exceptions;
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

    private const int BaseAutoCleanupTime = 2; // hours

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
            //size每100MB添加1小时
            var cleanupTime = BaseAutoCleanupTime + (int)Math.Ceiling(size / 100000000.0);
            metadata.ExpirationTime = DateTimeOffset.Now.AddHours(cleanupTime);
            await JsonSerializer.SerializeAsync(stream, metadata);

            return metadata;
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while creating metadata for hash: {Hash}", hash);
            throw;
        }
    }

    protected virtual async Task AddFileMetadataCacheAsync(string hash, long size, long chunkSize, string filename,
        string mimeType, string extension)
    {
        var cacheItem = new FileMetadataCacheItem(hash, filename, size, chunkSize, filename, extension);
        var cacheKey = await GetFileMetadataCacheNameAsync(hash);
        await FileMetadataCache.SetAsync(cacheKey, cacheItem);
    }

    protected virtual async Task<FileMetadataCacheItem> GetFileMetadataCacheAsync(string hash)
    {
        var cacheKey = await GetFileMetadataCacheNameAsync(hash);
        var metadata = await FileMetadataCache.GetAsync(cacheKey, token: CancellationToken);
        if (metadata == null)
        {
            throw new MetadataNotFoundBusinessException(hash);
        }

        return metadata;
    }

    protected virtual async Task RemoveFileMetadataCacheAsync(string hash)
    {
        var cacheKey = await GetFileMetadataCacheNameAsync(hash);
        await FileMetadataCache.RemoveAsync(cacheKey, token: CancellationToken);
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