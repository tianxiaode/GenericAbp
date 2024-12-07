using Microsoft.Extensions.Logging;
using SkiaSharp;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
using System.Security.Cryptography;
using Generic.Abp.Extensions.MimeDetective;
using Generic.Abp.FileManagement.Exceptions;

namespace Generic.Abp.FileManagement.FileInfoBases;

public partial class FileInfoBaseManager
{
    /// <summary>
    /// 合并文件块
    /// Merge file chunks
    /// </summary>
    /// <param name="hash">The hash value of the file</param>
    /// <param name="allowTypes">Allowed file types</param>
    /// <param name="tenantId"> Tenant Id</param>
    /// <returns></returns>
    /// <exception cref="FileChunkErrorBusinessException"></exception>
    public virtual async Task<(FileMetadataCacheItem, FileInfoBase)> MergeAsync(string hash, string allowTypes,
        Guid? tenantId = null)
    {
        var fileInfoBase = await GetAndCheckIsAllowedAsync(hash, allowTypes);
        //文件已经上传
        if (fileInfoBase != null)
        {
            return (await GetFileMetadataCacheAsync(hash), fileInfoBase);
        }

        var done = true;

        var metadata = await GetFileMetadataCacheAsync(hash, 0, -1, "", "", "");
        if (metadata.ChunkSize == -1)
        {
            throw new MetadataNotFoundBusinessException(hash);
        }

        var chunkDir = await GetAndCheckTempPathAsync(hash);

        //检查文件块是否完整
        for (var i = 0; i < metadata.TotalChunks; i++)
        {
            var filePath = Path.Combine(chunkDir, $"{hash}_{i}");
            if (File.Exists(filePath))
            {
                continue;
            }

            done = false;
            break;
        }

        if (!done)
        {
            throw new FileChunkErrorBusinessException();
        }

        try
        {
            await MergeChunkAsync(metadata, chunkDir);
            var entity = new FileInfoBase(GuidGenerator.Create(), hash, metadata.Mimetype, metadata.Extension,
                metadata.Size, await GetAccessPathAsync(hash), tenantId);
            await CreateAsync(entity);
            await CleanTempPathAsync(hash, chunkDir, metadata);
            return (metadata, entity);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "合并文件块失败");
            throw;
        }
    }


    protected virtual async Task MergeChunkAsync(FileMetadataCacheItem metadata, string chunkDir)
    {
        var hash = metadata.Hash;
        var finalFilePath = await GetStorageDirectoryAsync(hash);
        var success = false;

        try
        {
            const int maxConcurrency = 4; // 限制并发读取任务的数量
            var semaphore = new SemaphoreSlim(maxConcurrency);
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(CancellationToken);

            // 预分配文件空间
            await using (var fileStream =
                         new FileStream(finalFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                fileStream.SetLength(metadata.Size); // 设置文件长度为最终大小
            }

            // 启动并发读取和写入任务
            var writeTasks = new List<Task>();
            for (var i = 0; i < metadata.TotalChunks; i++)
            {
                var chunkPath = Path.Combine(chunkDir, $"{hash}_{i}");
                var chunkOffset = metadata.ChunkSize * i;

                writeTasks.Add(ProcessChunkAsync(metadata, chunkPath, chunkOffset, finalFilePath, semaphore,
                    cts.Token));
            }

            // 等待所有分片处理完成
            try
            {
                await Task.WhenAll(writeTasks);
            }
            catch (OperationCanceledException e)
            {
                Logger.LogError(e, "合并文件块被取消");
                throw;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "合并文件块失败");
                // 如果有任意任务失败，则取消所有任务
                if (!cts.IsCancellationRequested)
                {
                    await cts.CancelAsync();
                }

                throw;
            }

            // 计算整个文件的 MD5 值
            await ValidateFileHashAsync(metadata, finalFilePath, cts.Token);
            success = true;
        }
        catch (OperationCanceledException e)
        {
            Logger.LogError(e, "合并文件块被取消");
            // 操作被取消，可能是由于某个任务抛出异常导致的取消
            throw;
        }
        catch (Exception e)
        {
            // 合并失败，记录错误日志
            Logger.LogError(e, "合并文件块失败");
            throw;
        }
        finally
        {
            // 确保无论成功与否，都进行文件清理
            if (!success && File.Exists(finalFilePath))
            {
                File.Delete(finalFilePath);
            }
        }
    }

    protected virtual async Task ValidateFileHashAsync(FileMetadataCacheItem metadata, string finalFilePath,
        CancellationToken cancellationToken)
    {
        using var md5 = MD5.Create();
        await using var fileStream = new FileStream(finalFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        var hashValue = await md5.ComputeHashAsync(fileStream, cancellationToken);
        var actualHashHex = Convert.ToHexStringLower(hashValue);

        if (!string.Equals(actualHashHex, metadata.Hash, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"文件哈希校验失败，实际哈希: {actualHashHex}, 预期哈希: {metadata.Hash}");
        }
    }

    protected virtual async Task ProcessChunkAsync(FileMetadataCacheItem metadata, string chunkPath, long chunkOffset,
        string finalFilePath,
        SemaphoreSlim semaphore, CancellationToken cancellationToken)
    {
        await semaphore.WaitAsync(cancellationToken);
        try
        {
            Logger.LogInformation("开始处理分片: {chunkPath}", chunkPath);

            // 使用流式读取分片并直接写入最终文件的正确位置
            await using var chunkStream = new FileStream(chunkPath, FileMode.Open, FileAccess.Read);
            await using var finalFileStream =
                new FileStream(finalFilePath, FileMode.Open, FileAccess.Write, FileShare.None);

            finalFileStream.Seek(chunkOffset, SeekOrigin.Begin); // 移动到正确的偏移量

            // 确保不会超出文件总大小
            var remainingBytes = metadata.Size - chunkOffset;
            var buffer = new byte[Math.Min(remainingBytes, 81920)]; // 根据剩余字节数调整缓冲区大小

            int bytesRead;
            while ((bytesRead = await chunkStream.ReadAsync(buffer.AsMemory(), cancellationToken)) > 0)
            {
                if (chunkOffset + bytesRead > metadata.Size)
                {
                    // 如果当前写入会超出文件总大小，则只写入剩余的字节数
                    await finalFileStream.WriteAsync(buffer.AsMemory(0, (int)(metadata.Size - chunkOffset)),
                        cancellationToken);
                    break;
                }

                await finalFileStream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
            }
        }
        catch (OperationCanceledException e)
        {
            Logger.LogError(e, "分片处理被取消");
            // 处理取消的情况，这里可以选择抛出或不抛出
            throw;
        }
        catch (Exception e)
        {
            Logger.LogError(e, "分片处理失败");
            // 如果有任何一个任务失败，则取消所有任务
            throw;
        }
        finally
        {
            semaphore.Release();
        }
    }

    protected virtual async Task CleanTempPathAsync(string hash, string chunkDir, FileMetadataCacheItem metadata)
    {
// 清理分片文件
        foreach (var i in Enumerable.Range(0, metadata.TotalChunks))
        {
            var filePath = Path.Combine(chunkDir, $"{hash}_{i}");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        var metadataFilePath = await GetMetadataFileNameAsync(hash);
        if (File.Exists(metadataFilePath))
        {
            File.Delete(metadataFilePath);
        }

        await RemoveFileMetadataCacheAsync(hash);
    }


    protected virtual async Task<string> GetStorageDirectoryAsync(string hash)
    {
        var filenamePath = await GetAccessPathAsync(hash);
        var path = Path.Combine(filenamePath).Replace("\\", "/");
        var storageDirectory = await GetPhysicalPathAsync(path, true);
        var finalFilePath = Path.Combine(storageDirectory, await GetFileNameAsync(hash, ""));
        return finalFilePath;
    }


    protected virtual async Task ThumbnailAsync(string hash, string storageDirectory, long size, string mime,
        long thumbnailSize)
    {
        if (!MimeTypes.ImageTypes.Select(m => m.Mime).Contains(mime))
        {
            return;
        }

        if (size < thumbnailSize)
        {
            return;
        }

        var filePath = Path.Combine(storageDirectory, await GetFileNameAsync(hash, ""));
        await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var original = SKBitmap.Decode(fileStream);
        var width = original.Width;
        var height = original.Height;
        var factor = Math.Sqrt(width * height / 34000d);
        if (factor < 1)
        {
            return;
        }

        width = (int)(width / factor);
        height = (int)(height / factor);
        using var resized = original.Resize(new SKImageInfo(width, height), SKFilterQuality.High);
        if (resized == null)
        {
            return;
        }

        using var image = SKImage.FromBitmap(resized);
        var path = Path.Combine(storageDirectory, await GetThumbnailFileNameAsync(hash)).Replace("\\", "/");
        var thumbnail = new FileStream(path, FileMode.OpenOrCreate);
        image.Encode(SKEncodedImageFormat.Png, 100).SaveTo(thumbnail);
        fileStream.Close();
    }
}