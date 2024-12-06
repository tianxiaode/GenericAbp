using Generic.Abp.Extensions.Entities;
using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.Extensions.RemoteContents;
using Generic.Abp.FileManagement.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.Exceptions;
using Medallion.Threading;
using Volo.Abp.Caching;
using Volo.Abp.DistributedLocking;
using Volo.Abp.SettingManagement;
using Volo.Abp.Threading;
using File = System.IO.File;

namespace Generic.Abp.FileManagement.FileInfoBases;

public partial class FileInfoBaseManager(
    IFileInfoBaseRepository repository,
    ISettingManager settingManager,
    IStringLocalizer<FileManagementResource> localizer,
    ICancellationTokenProvider cancellationTokenProvider,
    IDistributedCache<FileMetadataCacheItem, string> fileMetadataCache,
    IDistributedCache<FileChunkStatusCacheItem, string> fileChunkStatusCache,
    IAbpDistributedLock distributedLock)
    : EntityManagerBase<FileInfoBase, IFileInfoBaseRepository, FileManagementResource>(repository, localizer,
        cancellationTokenProvider)
{
    protected ISettingManager SettingManager { get; } = settingManager;
    protected IDistributedCache<FileMetadataCacheItem, string> FileMetadataCache { get; } = fileMetadataCache;

    protected IDistributedCache<FileChunkStatusCacheItem, string> FileChunkStatusCache { get; } =
        fileChunkStatusCache;

    protected IAbpDistributedLock DistributedLock { get; } = distributedLock;

    public virtual async Task<FileInfoBase?> FindByHashAsync(string hash)
    {
        return await Repository.FindAsync(m => m.Hash == hash, false, CancellationToken);
    }


    public virtual async Task DeleteAsync(FileInfoBase entity, bool autoSave = true, bool deleteFileOnly = false)
    {
        //删除文件
        var path = entity.Path;
        var storageDirectory = await GetPhysicalPathAsync(path, false);
        var file = Path.Combine(storageDirectory, await GetFileNameAsync(entity.Hash, entity.Extension));
        if (File.Exists(storageDirectory))
        {
            File.Delete(file);
            var thumbnail = await GetThumbnailFileNameAsync(entity.Hash);
            var thumbnailFile = Path.Combine(storageDirectory, thumbnail);
            if (File.Exists(thumbnailFile))
            {
                File.Delete(thumbnailFile);
            }
        }

        if (!deleteFileOnly)
        {
            await DeleteAsync(entity, autoSave);
        }
    }

    public virtual async Task<IRemoteContent> GetFileAsync(FileInfoBase entity,
        int chunkSize = FileConsts.DefaultChunkSize,
        int? index = null)
    {
        var filename = $"{entity.Hash}.{entity.Extension}";
        var filePath = Path.Combine(await GetPhysicalPathAsync(entity.Path), filename);

        if (!File.Exists(filePath))
        {
            throw new EntityNotFoundBusinessException(L["File"], entity.Hash);
        }

        await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read,
            bufferSize: 4096, useAsync: true);
        var memoryStream = new MemoryStream();
        if (!index.HasValue)
        {
            // 返回整个文件
            await fileStream.CopyToAsync(memoryStream, CancellationToken);
            memoryStream.Position = 0;
            return new RemoteStreamContent(memoryStream, filename, "application/octet-stream", (int)fileStream.Length);
        }

        // 计算起始位置
        long startPosition = chunkSize * index.Value;
        if (startPosition >= fileStream.Length)
        {
            throw new EntityNotFoundBusinessException(L["File"], entity.Hash);
        }

        // 计算实际读取的字节数
        var actualChunkSize = (int)Math.Min(chunkSize, fileStream.Length - startPosition);

        // 根据文件大小决定返回类型
        if (fileStream.Length <= FileConsts.LargeFileSizeThreshold)
        {
            // 小文件，返回 RemoteByteContent
            var buffer = new byte[actualChunkSize];
            fileStream.Seek(startPosition, SeekOrigin.Begin);
            var i = await fileStream.ReadAsync(buffer, 0, actualChunkSize, CancellationToken);
            return new RemoteByteContent(buffer, filename, "application/octet-stream", actualChunkSize);
        }

        // 大文件，返回 RemoteStreamContent
        fileStream.Seek(startPosition, SeekOrigin.Begin);
        await fileStream.CopyToAsync(memoryStream, actualChunkSize, CancellationToken);
        memoryStream.Position = 0;
        return new RemoteStreamContent(memoryStream, filename, "application/octet-stream", actualChunkSize);
    }

    public virtual async Task<byte[]?> GetThumbnailAsync(FileInfoBase entity)
    {
        var filename = await GetThumbnailFileNameAsync(entity.Hash);
        var dir = await GetPhysicalPathAsync(entity.Path);
        var file = Path.Combine(dir, filename);
        if (File.Exists(file))
        {
            return await File.ReadAllBytesAsync(file, CancellationToken);
        }

        filename = $"{entity.Hash}.{entity.Extension}";
        file = Path.Combine(dir, filename);
        if (File.Exists(file))
        {
            return await System.IO.File.ReadAllBytesAsync(file, CancellationToken);
        }

        return null;
    }

    protected virtual async Task<FileInfoBase?> GetAndCheckIsAllowedAsync(string hash, string allowFileTypes)
    {
        var entity = await FindByHashAsync(hash);
        if (entity == null)
        {
            return null;
        }

        var fileTypes = allowFileTypes.Split(',');
        if (!fileTypes.Contains(entity.Extension))
        {
            throw new InvalidFileTypeBusinessException();
        }

        return entity;
    }
}