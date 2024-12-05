using Microsoft.Extensions.Logging;
using SkiaSharp;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
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
    /// <param name="totalChunks">Total chunks</param>
    /// <param name="allowTypes">Allowed file types</param>
    /// <param name="allowSize">Allowed file size</param>
    /// <param name="usedStorage">Used storage space</param>
    /// <param name="thumbnailSize">Thumbnail size, The default is 100k</param>
    /// <param name="storageQuota">Storage quota, The default is 100M</param>
    /// <returns></returns>
    /// <exception cref="FileChunkErrorBusinessException"></exception>
    public virtual async Task<FileInfoBase> MergeAsync(string hash, string fileName, int totalChunks,
        string allowTypes, long allowSize, long storageQuota, long usedStorage, long thumbnailSize = 102400)
    {
        var fileInfoBase = await FindByHashAsync(hash);
        //文件已经上传
        if (fileInfoBase != null)
        {
            return fileInfoBase;
        }

        var done = true;
        var dir = await GetTempPathAsync(hash);

        //检查文件块是否完整
        for (var i = 0; i < totalChunks; i++)
        {
            var filePath = Path.Combine(dir, $"{hash}_{i}");
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


        var dto = await SaveAsync(hash, fileName, totalChunks, dir, allowTypes, allowSize, storageQuota, usedStorage,
            thumbnailSize);
        if (Directory.Exists(dir))
        {
            Directory.Delete(dir, true);
        }


        return dto;
    }

    protected virtual async Task<FileInfoBase> SaveAsync(string hash, string fileName, int totalChunks, string tempDir,
        string allowTypes, long allowSize, long storageQuota, long usedStorage, long thumbnailSize)
    {
        // 创建最终合并后的临时文件路径
        var filenamePath = await GetAccessPathAsync(hash);
        var path = Path.Combine(filenamePath).Replace("\\", "/");
        var storageDirectory = await GetPhysicalPathAsync(path, true);
        var finalFilePath = Path.Combine(storageDirectory, await GetFileNameAsync(hash, ""));

        // 限制并发任务数量
        const int maxConcurrency = 4;
        var semaphore = new SemaphoreSlim(maxConcurrency);

        // 使用 FileStream 写入最终文件
        await using (var fileStream = new FileStream(finalFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            // 使用并发读取任务
            var tasks = new List<Task<(int ChunkIndex, byte[] Data)>>();

            for (var i = 0; i < totalChunks; i++)
            {
                var chunkIndex = i;
                var chunkPath = Path.Combine(tempDir, $"{hash}_{chunkIndex}");

                tasks.Add(Task.Run(async () =>
                {
                    await semaphore.WaitAsync(CancellationToken);
                    try
                    {
                        // 读取分片文件
                        Logger.LogInformation("开始读取分片: {chunkPath}", chunkPath);
                        var data = await File.ReadAllBytesAsync(chunkPath, CancellationToken);

                        // 校验分片大小（可选，具体逻辑可以根据需求调整）
                        if (data.Length == 0)
                        {
                            throw new InvalidOperationException($"分片 {chunkIndex} 是空的");
                        }

                        return (ChunkIndex: chunkIndex, Data: data);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }, CancellationToken));
            }

            // 等待所有读取任务完成
            var results = await Task.WhenAll(tasks);

            // 按 ChunkIndex 排序，保证写入顺序
            foreach (var result in results.OrderBy(r => r.ChunkIndex))
            {
                await fileStream.WriteAsync(result.Data.AsMemory(), CancellationToken);
            }

            // 验证文件大小
            var fileType = await fileStream.IsFileTypeAsync(await allowTypes.GetFileTypesAsync(), fileName);
            if (fileType == null)
            {
                throw new InvalidFileTypeBusinessException();
            }
        }


        // 检查文件大小
        var fileSize = new FileInfo(finalFilePath).Length;
        if (fileSize > allowSize)
        {
            throw new FileSizeOutOfRangeBusinessException(allowSize, fileSize);
        }

        if (storageQuota != 0 && fileSize > storageQuota - usedStorage)
        {
            throw new InsufficientStorageSpaceBusinessException(fileSize, usedStorage, storageQuota);
        }

        // 创建 FileInfoBase 实体
        var entity = new FileInfoBase(GuidGenerator.Create(), hash, fileType.Mime, fileType.Extension, fileSize,
            filenamePath, CurrentTenant.Id);

        // 生成缩略图
        await ThumbnailAsync(hash, storageDirectory, fileSize, fileType, thumbnailSize, null);

        await CreateAsync(entity);
        return entity;
    }

    protected virtual async Task ThumbnailAsync(string hash, string storageDirectory, long size, FileType fileType,
        long thumbnailSize, MemoryStream stream)
    {
        if (!MimeTypes.ImageTypes.Contains(fileType))
        {
            return;
        }

        if (size < thumbnailSize)
        {
            return;
        }

        using var original = SKBitmap.Decode(stream);
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
        stream.Close();
    }

    public virtual async Task SaveFileAsync(byte[] bytes, string dir, string filename)
    {
        dir = dir.Replace("\\", "/");
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var path = Path.Combine(dir, filename).Replace("\\", "/");
        Logger.LogInformation($"保存文件：{path}");
        await using var file = File.Create(path);
        await file.WriteAsync(bytes, CancellationToken);
        file.Close();
    }
}