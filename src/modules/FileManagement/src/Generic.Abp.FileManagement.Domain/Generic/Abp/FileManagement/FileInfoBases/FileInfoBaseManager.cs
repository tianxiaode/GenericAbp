using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.Extensions.MimeDetective;
using Generic.Abp.Extensions.RemoteContents;
using Generic.Abp.FileManagement.Exceptions;
using Generic.Abp.FileManagement.Localization;
using Generic.Abp.FileManagement.Settings;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.SettingManagement;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using File = System.IO.File;

namespace Generic.Abp.FileManagement.FileInfoBases;

public class FileInfoBaseManager(
    IFileInfoBaseRepository repository,
    ISettingManager settingManager,
    IStringLocalizer<FileManagementResource> localizer,
    ICancellationTokenProvider cancellationTokenProvider) : DomainService
{
    protected IFileInfoBaseRepository Repository { get; } = repository;
    protected ISettingManager SettingManager { get; } = settingManager;
    protected ICancellationTokenProvider CancellationTokenProvider { get; } = cancellationTokenProvider;
    protected CancellationToken CancellationToken => CancellationTokenProvider.Token;
    protected IStringLocalizer<FileManagementResource> Localizer { get; } = localizer;

    public virtual async Task<FileInfoBase?> FindByHashAsync(string hash)
    {
        return await Repository.FindAsync(m => m.Hash == hash, false, CancellationToken);
    }

    public virtual async Task<FileInfoBase> CreateAsync(FileInfoBase entity, bool autoSave = true)
    {
        await Repository.InsertAsync(entity, autoSave, CancellationToken);
        return entity;
    }

    public virtual async Task DeleteAsync(FileInfoBase entity, bool deleteFileOnly = false)
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
            await Repository.DeleteAsync(entity, true, CancellationToken);
        }
    }

    public virtual async Task<IRemoteContent> GetFileAsync(FileInfoBase entity,
        int chunkSize = FileConsts.DefaultChunkSize,
        int index = 0)
    {
        var filename = $"{entity.Hash}.{entity.Extension}";
        var filePath = Path.Combine(await GetPhysicalPathAsync(entity.Path), filename);

        if (!File.Exists(filePath))
        {
            throw new EntityNotFoundBusinessException(Localizer["File"], entity.Hash);
        }

        await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read,
            bufferSize: 4096, useAsync: true);
        // 计算起始位置
        long startPosition = chunkSize * index;
        if (startPosition >= fileStream.Length)
        {
            throw new EntityNotFoundBusinessException(Localizer["File"], entity.Hash);
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
        var memoryStream = new MemoryStream();
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


    /// <summary>
    /// 检查文件是否已上传
    /// Check the file upload
    /// </summary>
    /// <param name="hash">The hash value of the file</param>
    /// <param name="size">File size</param>
    /// <param name="chunkSize">File chunk size</param>
    /// <returns></returns>
    [UnitOfWork]
    public virtual async Task<ICheckFileResult> CheckAsync(string hash, long size,
        int chunkSize = FileConsts.DefaultChunkSize)
    {
        var fileInfoBase = await FindByHashAsync(hash);
        var result = new FileCheckResult(hash);
        if (fileInfoBase != null)
        {
            result.SetFile(fileInfoBase);
            return result;
        }

        var totalChunks = size / chunkSize;
        if (totalChunks * chunkSize < size)
        {
            totalChunks++;
        }

        result.Uploaded = new Dictionary<int, bool>();
        var dir = await GetTempPathAsync(hash);
        for (var i = 0; i < totalChunks; i++)
        {
            var filePath = Path.Combine(dir, $"{hash}_{i}").Replace("\\", "/");
            result.Uploaded.Add(i, File.Exists(filePath));
        }

        return result;
    }

    /// <summary>
    /// 上传文件块
    /// Upload a chunk of files
    /// </summary>
    /// <param name="hash">The hash value of the file</param>
    /// <param name="chunkBytes">A byte array of upload chunk</param>
    /// <param name="index">The index of the upload chunk</param>
    /// <returns></returns>
    /// <exception cref="TheFileNameCannotBeEmptyBusinessException"></exception>
    /// <exception cref="ValueExceedsFieldLengthBusinessException"></exception>
    [UnitOfWork]
    public virtual async Task UploadChunkAsync(string hash, byte[] chunkBytes, int index)
    {
        //如果文件存在，直接返回
        var exits = await FindByHashAsync(hash);
        if (exits != null)
        {
            return;
        }

        //保存文件
        var dir = await GetTempPathAsync(hash);
        Logger.LogInformation($"Chunk size:{chunkBytes.Length}");
        await SaveFileAsync(chunkBytes, dir, $"{hash}_{index}");
    }

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
    [UnitOfWork]
    public virtual async Task<FileInfoBase> MergeAsync(string hash, int totalChunks,
        List<FileType> allowTypes, long allowSize, long storageQuota, long usedStorage, long thumbnailSize = 102400)
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


        var dto = await SaveAsync(hash, totalChunks, dir, allowTypes, allowSize, storageQuota, usedStorage,
            thumbnailSize);
        if (Directory.Exists(dir))
        {
            Directory.Delete(dir, true);
        }


        return dto;
    }

    protected virtual async Task<FileInfoBase> SaveAsync(string hash, int totalChunks, string tempDir,
        List<FileType> allowTypes, long allowSize, long storageQuota, long usedStorage, long thumbnailSize)
    {
        await using var memorySteam = new MemoryStream();
        for (var i = 0; i < totalChunks; i++)
        {
            var filePath = Path.Combine(tempDir, $"{hash}_{i}");
            Logger.LogInformation($"文件合并:{filePath}");
            var bytes = await File.ReadAllBytesAsync(filePath, CancellationToken);
            Logger.LogInformation($"文件合并:{bytes.Length}");
            await memorySteam.WriteAsync(bytes, 0, bytes.Length, CancellationToken);
        }

        var fileType = await memorySteam.ToArray().GetFileTypeAsync(allowTypes);
        if (fileType == null)
        {
            throw new InvalidFileTypeBusinessException();
        }

        var fileSize = memorySteam.Length;
        if (fileSize > allowSize)
        {
            throw new FileSizeOutOfRangeBusinessException(allowSize, fileSize);
        }

        if (storageQuota != 0 && fileSize > storageQuota - usedStorage)
        {
            throw new InsufficientStorageSpaceBusinessException(fileSize, usedStorage, storageQuota);
        }


        var filenamePath = await GetAccessPathAsync(hash);
        var path = Path.Combine(filenamePath).Replace("\\", "/");
        var storageDirectory = await GetPhysicalPathAsync(path, true);
        await SaveFileAsync(await memorySteam.GetAllBytesAsync(CancellationToken), storageDirectory,
            await GetFileNameAsync(hash, fileType.Extension));
        memorySteam.Close();

        var entity = new FileInfoBase(GuidGenerator.Create(), hash, fileType.Mime, fileType.Extension, fileSize,
            filenamePath, CurrentTenant.Id);
        await ThumbnailAsync(hash, storageDirectory, fileSize, fileType, thumbnailSize, memorySteam);

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

    protected virtual async Task<string> GetTempPathAsync(string hash)
    {
        var storagePath = await GetStoragePathAsync();
        return Path.Combine(Directory.GetCurrentDirectory(), storagePath, "temp", hash);
    }

    public virtual Task<string> GetAccessPathAsync(string hash)
    {
        return Task.FromResult($"{hash[..2]}/{hash.Substring(2, 2)}/{hash.Substring(4, 2)}");
    }

    public virtual async Task<string> GetPhysicalPathAsync(string path, bool isCreated = false)
    {
        var storagePath = await GetStoragePathAsync();
        var dir = Path.Combine(Directory.GetCurrentDirectory(), storagePath, path);
        if (isCreated && !Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        return dir;
    }

    public virtual Task<string> GetFileNameAsync(string hash, string extension)
    {
        return Task.FromResult($"{hash}.{extension}");
    }

    protected virtual Task<string> GetThumbnailFileNameAsync(string hash)
    {
        return Task.FromResult($"{hash}_thumbnail.png");
    }

    public virtual async Task<string> GetStoragePathAsync()
    {
        return await SettingManager.GetOrNullForCurrentTenantAsync(FileManagementSettings.StoragePath);
    }
}