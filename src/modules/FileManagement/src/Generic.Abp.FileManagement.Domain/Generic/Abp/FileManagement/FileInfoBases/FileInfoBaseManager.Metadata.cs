using Microsoft.Extensions.Caching.Distributed;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Medallion.Threading;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace Generic.Abp.FileManagement.FileInfoBases;

public partial class FileInfoBaseManager
{
    public virtual async Task<FileMetadataCacheItem> GetMetadataAsync(string hash, long size, long chunkSize,
        string filename)
    {
        var cacheKey = await GetCacheKeyAsync(hash);
        var cacheItem = await Cache.GetOrAddAsync(cacheKey,
            async () => await LoadMetaData(hash, size, chunkSize, filename),
            () => new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromHours(5)
            },
            token: CancellationToken);
        if (cacheItem == null)
        {
            throw new AbpException("File metadata not found.");
        }

        return cacheItem;
    }

    protected virtual async Task<FileMetadataCacheItem> LoadMetaData(string hash, long size, long chunkSize,
        string filename)
    {
        var dir = await GetAndCheckTempPathAsync(hash);
        var metadataPath = Path.Combine(dir, $"{hash}_metadata.json");

        try
        {
            if (!File.Exists(metadataPath))
            {
                return await CreateMetadataAsync(hash, size, chunkSize, filename, dir);
            }

            var fileData = await File.ReadAllTextAsync(metadataPath);
            var metadata = JsonSerializer.Deserialize<FileMetadataCacheItem>(fileData)
                           ?? throw new InvalidOperationException("JSON 元数据解析失败");

            if (metadata.Size != size || metadata.ChunkSize != chunkSize || metadata.FileName != filename)
            {
                throw new InvalidOperationException("JSON 元数据与当前上传信息不匹配");
            }

            return metadata;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "加载元数据失败，Hash: {Hash}", hash);
            throw;
        }
    }

    protected virtual async Task<FileMetadataCacheItem> CreateMetadataAsync(string hash, long size, long chunkSize,
        string filename, string dir)
    {
        var lockKey = $"file_metadata_lock:{hash}";

        // 使用分布式锁
        await using var handle = await DistributedLockProvider.TryAcquireLockAsync(lockKey, TimeSpan.FromSeconds(30));
        if (handle == null)
        {
            throw new AbpException("无法获取分布式锁，请稍后重试");
        }

        var metadataPath = Path.Combine(dir, await GetMetadataFileNameAsync(hash));

        // 再次检查文件是否存在
        if (File.Exists(metadataPath))
        {
            var fileData = await File.ReadAllTextAsync(metadataPath);
            return JsonSerializer.Deserialize<FileMetadataCacheItem>(fileData)
                   ?? throw new InvalidOperationException("JSON 元数据解析失败");
        }

        var metadata = new FileMetadataCacheItem(hash, size, chunkSize, filename);
        var cacheData = JsonSerializer.Serialize(metadata);

        // 持久化到 JSON 文件
        await File.WriteAllTextAsync(metadataPath, cacheData);
        return metadata;
    }

    public virtual async Task RemoveMetadataAsync(string hash)
    {
        var cacheKey = await GetCacheKeyAsync(hash);
        await Cache.RemoveAsync(cacheKey, token: CancellationToken);

        var dir = await GetAndCheckTempPathAsync(hash);
        var metadataPath = Path.Combine(dir, await GetMetadataFileNameAsync(hash));

        try
        {
            if (File.Exists(metadataPath))
            {
                File.Delete(metadataPath);
            }
        }
        catch (IOException ex)
        {
            Logger.LogWarning(ex, "删除元数据文件失败，Hash: {Hash}", hash);
        }
    }

    protected virtual Task<string> GetCacheKeyAsync(string hash)
    {
        return Task.FromResult($"file_metadata:{hash}");
    }

    protected virtual Task<string> GetMetadataFileNameAsync(string hash)
    {
        return Task.FromResult($"{hash}_metadata.json");
    }
}