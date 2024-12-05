using Generic.Abp.Extensions.MimeDetective;
using Generic.Abp.FileManagement.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Abp.FileManagement.FileInfoBases;

public partial class FileInfoBaseManager
{
    /// <summary>
    /// 检查文件是否已上传
    /// </summary>
    public virtual async Task<ICheckFileResult> CheckAsync(string hash, long size,
        int chunkSize = FileConsts.DefaultChunkSize)
    {
        // 1. 验证输入参数
        await ValidateChunkSize(chunkSize);

        // 2. 检查文件是否已存在
        var fileInfoBase = await FindByHashAsync(hash);
        var result = new FileCheckResult(hash);
        if (fileInfoBase != null)
        {
            result.SetFile(fileInfoBase);
            return result;
        }

        // 3. 计算总分片数
        var totalChunks = await CalculateTotalChunks(size, chunkSize);

        // 4. 检查临时目录
        var dir = await GetAndCheckTempPathAsync(hash);

        // 5. 记录分片上传状态（并发检查文件是否存在）
        var tasks = Enumerable.Range(0, totalChunks)
            .Select(async i =>
            {
                var filePath = Path.Combine(dir, $"{hash}_{i}");
                return new { Index = i, Exists = await Task.Run(() => File.Exists(filePath)) };
            });

        var results = await Task.WhenAll(tasks);
        result.Uploaded = results.ToDictionary(r => r.Index, r => r.Exists);

        return result;
    }

    /// <summary>
    /// 上传文件块
    /// </summary>
    public virtual async Task UploadChunkAsync(string hash, byte[] chunkBytes, int index, string allowFileTypes)
    {
        // 如果文件已经存在，直接返回
        var exists = await FindByHashAsync(hash);
        if (exists != null)
        {
            return;
        }

        // 保存分片到临时目录
        var dir = await GetAndCheckTempPathAsync(hash);

        // 计算分片的哈希值并保存
        var chunkHash = await ComputeChunkHash(chunkBytes);
        await SaveChunkHashAsync(hash, index, chunkHash, dir);

        // 如果是第一个分片（index == 0），检查文件类型
        if (index == 0)
        {
            var fileType = await DetectFileTypeFromFirstChunkAsync(chunkBytes, allowFileTypes, dir);
        }

        Logger.LogInformation("Chunk size: {chunkBytes.Length}", chunkBytes.Length);
        await SaveFileAsync(chunkBytes, dir, $"{hash}_{index}");
    }

    public async Task<string> DetectFileTypeFromFirstChunkAsync(byte[] chunkBytes, string allowedTypes, string dir)
    {
        if (chunkBytes.Length < 1024) // 小于 1KB 分片
        {
            Logger.LogWarning("First chunk is smaller than 1KB: {chunkBytes.Length}", chunkBytes.Length);
        }

        // 检测文件类型
        var fileType = await chunkBytes.GetFileTypeAsync(await allowedTypes.GetFileTypesAsync());
        if (fileType == null)
        {
            await DeleteAllChunksAsync(dir);
            throw new InvalidFileTypeBusinessException();
        }

        Logger.LogInformation("检测到文件类型：{fileType.Mime}, 扩展名：{fileType.Extension}", fileType.Mime, fileType.Extension);

        return fileType.Mime;
    }

    protected virtual async Task DeleteAllChunksAsync(string tempDir)
    {
        if (Directory.Exists(tempDir))
        {
            var files = Directory.GetFiles(tempDir);
            const int batchSize = 100;

            for (var i = 0; i < files.Length; i += batchSize)
            {
                var batch = files.Skip(i).Take(batchSize);
                var deleteTasks = batch.Select(file => Task.Run(() => File.Delete(file)));
                await Task.WhenAll(deleteTasks);

                Logger.LogInformation("已删除文件：{deletedCount}/{totalCount}，当前目录：{tempDir}",
                    Math.Min(i + batchSize, files.Length), files.Length, tempDir);
            }

            // 删除空目录
            try
            {
                Directory.Delete(tempDir);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "删除目录失败：{tempDir}", tempDir);
            }
        }
    }

    protected virtual Task<int> CalculateTotalChunks(long fileSize, long chunkSize) =>
        Task.FromResult((int)Math.Ceiling((double)fileSize / chunkSize));

    protected virtual Task ValidateChunkSize(long chunkSize)
    {
        const long minChunkSize = 64 * 1024; // 64KB
        const long maxChunkSize = 16 * 1024 * 1024; // 16MB

        if (chunkSize is < minChunkSize or > maxChunkSize)
        {
            throw new InvalidChunkSizeBusinessException(minChunkSize, maxChunkSize, chunkSize);
        }

        return Task.CompletedTask;
    }

    protected virtual Task ValidateChunkIndex(int chunkIndex, int totalChunks)
    {
        if (chunkIndex < 0 || chunkIndex >= totalChunks)
        {
            throw new InvalidChunkIndexBusinessException(totalChunks, chunkIndex);
        }

        return Task.CompletedTask;
    }

    public virtual Task<string> ComputeChunkHash(byte[] chunkBytes)
    {
        var hashBytes = MD5.HashData(chunkBytes);
        return Task.FromResult(Convert.ToHexStringLower(hashBytes));
    }

    protected static readonly SemaphoreSlim HashFileLock = new(1, 1);

    protected virtual async Task SaveChunkHashAsync(string hash, int index, string chunkHash, string dir)
    {
        var hashManifestPath = Path.Combine(dir, "hash_manifest.json");

        // 使用文件锁避免并发冲突
        await HashFileLock.WaitAsync();
        try
        {
            Dictionary<int, string>? hashData = null;
            if (File.Exists(hashManifestPath))
            {
                hashData = JsonSerializer.Deserialize<Dictionary<int, string>>(
                    await File.ReadAllTextAsync(hashManifestPath));
            }

            hashData ??= new Dictionary<int, string>();
            hashData[index] = chunkHash;
            await File.WriteAllTextAsync(hashManifestPath, JsonSerializer.Serialize(hashData));
        }
        finally
        {
            HashFileLock.Release();
        }
    }
}