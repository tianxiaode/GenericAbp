using Generic.Abp.Extensions.MimeDetective;
using Generic.Abp.FileManagement.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Generic.Abp.FileManagement.FileInfoBases;

public partial class FileInfoBaseManager
{
    public const long MinChunkSize = 64 * 1024; // 64KB
    public const long MaxChunkSize = 16 * 1024 * 1024; // 16MB

    /// <summary>
    /// 检查文件是否已上传
    /// </summary>
    public virtual async Task<ICheckFileResult> CheckAsync(string hash, string filename,
        string allowFileTypes, long size, long allowSize, string chunkHash, int chunkSize, byte[] firstChunkBytes,
        long storageQuota, long usedStorage)
    {
        // 1. 验证输入参数
        await ValidateInputAsync(filename, allowFileTypes, size, allowSize, chunkSize,
            firstChunkBytes.Length, chunkHash, firstChunkBytes, 0, storageQuota, usedStorage);

        // 2. 检查文件是否已存在且是否允许上传的文件类型
        var fileInfoBase = await GetAndCheckIsAllowedAsync(hash, allowFileTypes);
        var result = new FileCheckResult(hash);
        if (fileInfoBase != null)
        {
            result.SetFile(fileInfoBase);
            return result;
        }

        // 3. 检查文件类型
        var filetype =
            await DetectFileTypeFromFirstChunkAsync(firstChunkBytes, allowFileTypes, await GetTempPathAsync(hash));

        await using var handle =
            await DistributedLock.TryAcquireAsync(await GetDistributedLockNameAsync(hash));
        if (handle == null)
        {
            throw new OtherUserUploadingTheFileBusinessException();
        }

        // 4. 保存文件元数据
        var metadata =
            await GetFileMetadataCacheAsync(hash, size, chunkSize, filename, filetype.Mime, filetype.Extension);
        var totalChunks = metadata.TotalChunks;
        await SaveChunkAsync(hash, 0, firstChunkBytes, chunkHash, totalChunks);
        // 5. 记录分片上传状态（并发检查文件是否存在）
        result.Uploaded = (await GetCheckChunkStatusAsync(hash, totalChunks)).ChunkStatuses;
        result.Uploaded[0] = true;
        return result;
    }


    /// <summary>
    /// 上传文件块
    /// </summary>
    public virtual async Task UploadChunkAsync(string hash, byte[] chunkBytes, int index, string chunkHash,
        string allowFileTypes)
    {
        // 再次检查文件是否已存在且是否允许上传的文件类型
        var exists = await GetAndCheckIsAllowedAsync(hash, allowFileTypes);
        if (exists != null)
        {
            return;
        }

        // 验证分片
        var totalChunk = await ValidateChunkAsync(hash, index, chunkBytes, chunkHash);

        // 尝试获取分布式锁
        await using var handle = await DistributedLock.TryAcquireAsync(await GetDistributedLockNameAsync(hash, index));
        if (handle == null)
        {
            throw new OtherUserUploadingTheFileBusinessException();
        }

        try
        {
            // 保存分片
            await SaveChunkAsync(hash, index, chunkBytes, chunkHash, totalChunk);

            Logger.LogInformation("Chunk size: {chunkSize}", chunkBytes.Length);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while uploading chunk for hash: {Hash}, Index: {Index}", hash, index);
            throw;
        }
    }

    public async Task<FileType> DetectFileTypeFromFirstChunkAsync(byte[] chunkBytes, string allowedTypes, string dir)
    {
        // 检测文件类型
        var fileType = await chunkBytes.GetFileTypeAsync(await allowedTypes.GetFileTypesAsync());
        if (fileType == null)
        {
            throw new InvalidFileTypeBusinessException();
        }

        Logger.LogInformation("检测到文件类型：{fileType.Mime}, 扩展名：{fileType.Extension}", fileType.Mime, fileType.Extension);

        return fileType;
    }


    protected virtual Task ValidateInputAsync(string filename, string allowFileTypes, long size, long allowSize,
        long chunkSize, long firsChunkSize, string chunkHash, byte[] chunkBytes, int index, long storageQuota,
        long usedStorage)
    {
        var fileType = allowFileTypes.Split(",");
        if (!fileType.Contains(Path.GetExtension(filename)))
        {
            throw new InvalidFileTypeBusinessException();
        }

        if (size > allowSize)
        {
            throw new FileSizeOutOfRangeBusinessException(allowSize, size);
        }

        if (size > storageQuota - usedStorage)
        {
            throw new InsufficientStorageSpaceBusinessException(size, usedStorage, storageQuota);
        }

        if (chunkSize is < MinChunkSize or > MaxChunkSize)
        {
            throw new InvalidChunkSizeBusinessException(MinChunkSize, MaxChunkSize, chunkSize);
        }

        if (firsChunkSize > chunkSize)
        {
            throw new InvalidChunkSizeBusinessException(0, chunkSize, firsChunkSize);
        }

        return Task.CompletedTask;
    }

    protected virtual async Task<int> ValidateChunkAsync(string hash, int chunkIndex, byte[] chunkBytes,
        string chunkHash)
    {
        var metadata = await GetFileMetadataCacheAsync(hash, 0, 0, "", "", "");
        var chunkSize = chunkBytes.Length;
        if (metadata.ChunkSize == 0)
        {
            throw new MetadataNotFoundBusinessException(hash);
        }

        if (chunkIndex >= metadata.TotalChunks)
        {
            throw new InvalidChunkIndexBusinessException(metadata.TotalChunks, chunkIndex);
        }

        if ((chunkIndex == metadata.TotalChunks - 1 && metadata.ChunkSize < chunkSize))
        {
            throw new InvalidChunkSizeBusinessException(0, metadata.ChunkSize, chunkSize);
        }

        if (chunkIndex < metadata.TotalChunks && metadata.ChunkSize != chunkSize)
        {
            throw new InvalidChunkSizeBusinessException(metadata.ChunkSize, metadata.ChunkSize, chunkSize);
        }

        return metadata.TotalChunks;
    }


    protected virtual async Task SaveChunkAsync(string hash, int index, byte[] chunkBytes, string expectedChunkHash,
        int totalChunks)
    {
        var chunkFilePath = string.Empty;
        var md5FilePath = string.Empty;

        try
        {
            // 计算分片的MD5值并立即验证
            var md5Hash = await ComputeAndValidateChunkHashAsync(chunkBytes, expectedChunkHash, index);

            // 保存分片到临时目录下的chunks下
            var (chunkDir, chunkMd5Dir) = await GetChunkPathAsync(hash);
            chunkFilePath = Path.Combine(chunkDir, $"{hash}_{index}");

            // 使用异步方式写入文件
            await File.WriteAllBytesAsync(chunkFilePath, chunkBytes);

            // 保存分片的MD5值到临时目录下的md5下，以二进制格式保存
            md5FilePath = Path.Combine(chunkMd5Dir, $"{hash}_{index}");

            // 直接将MD5哈希值作为字节数组写入文件
            await File.WriteAllBytesAsync(md5FilePath, md5Hash);
            await UpdateChunkStatusCacheAsync(hash, index, totalChunks);
        }
        catch (Exception e)
        {
            // 清理已创建的文件（如果存在）
            if (!string.IsNullOrEmpty(chunkFilePath) && File.Exists(chunkFilePath))
            {
                try
                {
                    File.Delete(chunkFilePath);
                }
                catch (IOException ioEx)
                {
                    Logger.LogError(ioEx, "Failed to delete chunk file: {chunkFilePath}", chunkFilePath);
                }
            }

            if (string.IsNullOrEmpty(md5FilePath) || !File.Exists(md5FilePath))
            {
                throw;
            }

            try
            {
                File.Delete(md5FilePath);
            }
            catch (IOException ioEx)
            {
                Logger.LogError(ioEx, "Failed to delete MD5 file: {md5FilePath}", md5FilePath);
            }

            throw;
        }
    }

    protected virtual async Task<byte[]> ComputeAndValidateChunkHashAsync(byte[] chunkBytes, string expectedChunkHash,
        int index)
    {
        using var stream = new MemoryStream(chunkBytes);
        var buffer = new byte[16]; // MD5 produces a 128-bit hash, or 16 bytes.

        // Compute the hash and write it into the buffer.
        var bytesWritten = await MD5.HashDataAsync(stream, buffer.AsMemory());

        if (bytesWritten != buffer.Length)
        {
            throw new InvalidOperationException("Unexpected number of bytes written for the hash.");
        }

        var actualHashHex = Convert.ToHexString(buffer).ToLowerInvariant();

        if (!string.Equals(actualHashHex, expectedChunkHash, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidChunkHashBusinessException(index, actualHashHex, expectedChunkHash);
        }

        return buffer;
    }

    protected virtual async Task<FileChunkStatusCacheItem> GetCheckChunkStatusAsync(string hash, int totalChunks)
    {
        var cacheKey = await GetFileChunkStatusCacheKeyAsync(hash);
        var cacheItem = await FileChunkStatusCache.GetOrAddAsync(cacheKey,
            async () => await CheckChunkStatusAsync(hash, totalChunks),
            () => new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddHours(5)
            },
            token: CancellationToken
        );
        if (cacheItem == null)
        {
            throw new Exception($"Cache item {cacheKey} is null");
        }

        return cacheItem;
    }

    protected virtual async Task UpdateChunkStatusCacheAsync(string hash, int index, int totalChunks)
    {
        var cacheKey = await GetFileChunkStatusCacheKeyAsync(hash);
        var cacheItem = await GetCheckChunkStatusAsync(hash, totalChunks);
        if (cacheItem.ChunkStatuses.ContainsKey(index))
        {
            cacheItem.ChunkStatuses[index] = true;
        }

        await FileChunkStatusCache.SetAsync(cacheKey, cacheItem);
    }

    protected virtual async Task<FileChunkStatusCacheItem> CheckChunkStatusAsync(string hash, int totalChunks)
    {
        var result = new FileChunkStatusCacheItem();
        // 定位 md5 目录
        var md5Dir = Path.Combine(await GetTempPathAsync(hash), "md5");

        // 如果 md5 目录不存在，则所有分片都未完成
        if (!Directory.Exists(md5Dir))
        {
            result.ChunkStatuses = Enumerable.Range(0, totalChunks).ToDictionary(i => i, _ => false);
            return result;
        }

        // 获取所有 MD5 文件 (hash_i_md5Value 格式)
        var md5Files = Directory.EnumerateFiles(md5Dir)
            .Select(Path.GetFileName)
            .ToList();

        // 构建分片完成状态
        var chunkStatus = new Dictionary<int, bool>();
        foreach (var i in Enumerable.Range(0, totalChunks))
        {
            var expectedFilePrefix = $"{hash}_{i}_";
            chunkStatus[i] = md5Files.Any(file => !file.IsNullOrEmpty() && file.StartsWith(expectedFilePrefix));
        }

        result.ChunkStatuses = chunkStatus;
        return result;
    }

    protected virtual Task<string> GetFileChunkStatusCacheKeyAsync(string hash)
    {
        return Task.FromResult($"FileManagement_FileInfoBaseManager_file_chunk_status_{hash}");
    }

    protected virtual Task<string> GetDistributedLockNameAsync(string hash, int chunkIndex = 0)
    {
        return Task.FromResult($"FileManagement_FileInfoBaseManager_DistributedLock_{hash}_{chunkIndex}_lock");
    }

    protected virtual async Task<(string, string)> GetChunkPathAsync(string hash)
    {
        return (await GetAndCheckTempPathAsync(hash, "chunks"), await GetAndCheckTempPathAsync(hash, "md5"));
    }
}