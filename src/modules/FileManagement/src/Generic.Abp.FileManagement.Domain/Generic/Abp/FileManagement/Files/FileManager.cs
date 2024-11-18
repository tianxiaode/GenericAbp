﻿using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.Extensions.Extensions;
using Generic.Abp.Extensions.MimeDetective;
using Generic.Abp.Extensions.RemoteContents;
using Generic.Abp.FileManagement.Exceptions;
using Generic.Abp.FileManagement.Folders;
using Generic.Abp.FileManagement.Localization;
using Generic.Abp.FileManagement.Settings;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.SettingManagement;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Generic.Abp.FileManagement.Files;

public class FileManager(
    IFileRepository repository,
    ICancellationTokenProvider cancellationTokenProvider,
    FilePermissionManager permissionManager,
    IdentityUserManager userManager,
    ISettingManager settingManager,
    IStringLocalizer<FileManagementResource> localizer)
    : DomainService
{
    protected IFileRepository Repository { get; } = repository;
    protected ICancellationTokenProvider CancellationTokenProvider { get; } = cancellationTokenProvider;
    protected CancellationToken CancellationToken => CancellationTokenProvider.Token;
    protected FilePermissionManager PermissionManager { get; } = permissionManager;
    protected IdentityUserManager UserManager { get; } = userManager;
    protected ISettingManager SettingManager { get; } = settingManager;
    protected IStringLocalizer<FileManagementResource> Localizer { get; } = localizer;

    [UnitOfWork]
    public virtual Task<File> CreateAsync(File entity, bool autoSave = true)
    {
        return Repository.InsertAsync(entity, autoSave, CancellationToken);
    }

    [UnitOfWork]
    public virtual Task<File> UpdateAsync(File entity, bool autoSave = true)
    {
        return Repository.UpdateAsync(entity, autoSave, CancellationToken);
    }

    [UnitOfWork]
    public virtual Task DeleteAsync(File entity, bool autoSave = true)
    {
        return Repository.DeleteAsync(m => m.Id == entity.Id, autoSave, CancellationToken);
    }


    [UnitOfWork]
    public virtual Task<File> GetAsync(Guid id)
    {
        return Repository.GetAsync(id, false, CancellationToken);
    }

    [UnitOfWork]
    public virtual Task<File?> FindAsync(Expression<Func<File, bool>> predicate)
    {
        return Repository.FirstOrDefaultAsync(predicate, CancellationToken);
    }

    [UnitOfWork]
    public virtual async Task<File?> FindByHashAsync(string hash)
    {
        if (!hash.IsAscii())
        {
            return null;
        }

        return await Repository.FirstOrDefaultAsync(m => m.Hash.Equals(hash), CancellationToken);
    }

    #region Get File

    public virtual async Task<IRemoteContent> GetFileAsync(File entity, int chunkSize = FileConsts.DefaultChunkSize,
        int index = 0)
    {
        var filename = $"{entity.Hash}.{entity.FileType}";
        var filePath = Path.Combine(await GetPhysicalPathAsync(entity.Path), filename);

        if (!System.IO.File.Exists(filePath))
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

    public virtual async Task<byte[]?> GetThumbnailAsync(File entity)
    {
        var filename = await GetThumbnailFileNameAsync(entity.Hash);
        var dir = await GetPhysicalPathAsync(entity.Path);
        var file = Path.Combine(dir, filename);
        if (System.IO.File.Exists(file))
        {
            return await System.IO.File.ReadAllBytesAsync(file, CancellationToken);
        }

        filename = $"{entity.Hash}.{entity.FileType}";
        file = Path.Combine(dir, filename);
        if (System.IO.File.Exists(file))
        {
            return await System.IO.File.ReadAllBytesAsync(file, CancellationToken);
        }

        return null;
    }

    #endregion


    #region File upload

    /// <summary>
    /// 检查文件是否已上传
    /// Check the file upload
    /// </summary>
    /// <typeparam name="TFileCheckResult"></typeparam>
    /// <param name="hash">The hash value of the file</param>
    /// <param name="size">File size</param>
    /// <param name="chunkSize">File chunk size</param>
    /// <returns></returns>
    [UnitOfWork]
    public virtual async Task<TFileCheckResult?> CheckAsync<TFileCheckResult>(string hash, int size,
        int chunkSize = FileConsts.DefaultChunkSize) where TFileCheckResult : FileCheckResult
    {
        var file = await FindByHashAsync(hash);
        var result = new FileCheckResult(hash);
        if (file != null)
        {
            result.SetFile(file);
            return result as TFileCheckResult;
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
            result.Uploaded.Add(i, System.IO.File.Exists(filePath));
        }

        return result as TFileCheckResult;
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
    /// <param name="uploadPath">Save the path</param>
    /// <param name="filename">filename</param>
    /// <param name="allowTypes">Allowed file types</param>
    /// <param name="allowSize">Allowed file size</param>
    /// <param name="thumbnailSize">Thumbnail size, The default is 100k</param>
    /// <returns></returns>
    /// <exception cref="FileChunkErrorBusinessException"></exception>
    [UnitOfWork]
    public virtual async Task<File> MergeAsync(string hash, int totalChunks, string uploadPath,
        string filename,
        List<FileType> allowTypes, int allowSize, long thumbnailSize = 102400)
    {
        var exits = await FindByHashAsync(hash);
        if (exits != null)
        {
            return exits;
        }

        var done = true;
        var dir = await GetTempPathAsync(hash);

        //检查文件块是否完整
        for (var i = 0; i < totalChunks; i++)
        {
            var filePath = Path.Combine(dir, $"{hash}_{i}");
            if (System.IO.File.Exists(filePath))
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


        var dto = await SaveAsync(hash, totalChunks, uploadPath, filename, dir, allowTypes, allowSize,
            thumbnailSize);
        if (Directory.Exists(dir))
        {
            Directory.Delete(dir, true);
        }


        return dto;
    }

    [UnitOfWork]
    protected virtual async Task<File> SaveAsync(string hash, int totalChunks, string uploadPath,
        string filename,
        string tempDir, List<FileType> allowTypes, int allowSize, long thumbnailSize)
    {
        await using var memorySteam = new MemoryStream();
        for (var i = 0; i < totalChunks; i++)
        {
            var filePath = Path.Combine(tempDir, $"{hash}_{i}");
            Logger.LogInformation($"文件合并:{filePath}");
            var bytes = await System.IO.File.ReadAllBytesAsync(filePath, CancellationToken);
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

        var filenamePath = await GetAccessPathAsync(hash);
        var path = Path.Combine(uploadPath, filenamePath).Replace("\\", "/");
        var storageDirectory = await GetPhysicalPathAsync(path, true);
        await SaveFileAsync(await memorySteam.GetAllBytesAsync(CancellationToken), storageDirectory,
            $"{hash}.{fileType.Extension}");
        memorySteam.Close();

        var entity = new File(GuidGenerator.Create(), hash, fileType.Mime, fileType.Extension, fileSize);
        entity.SetFilename(filename);
        entity.SetDescription(filename);
        entity.SetPath(path);
        await ThumbnailAsync(hash, storageDirectory, fileSize, fileType, thumbnailSize, memorySteam);

        await CreateAsync(entity);
        return entity;
    }

    public virtual async Task SaveFileAsync(byte[] bytes, string dir, string filename)
    {
        dir = dir.Replace("\\", "/");
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, filename).Replace("\\", "/");
        Logger.LogInformation($"保存文件：{path}");
        await using var file = System.IO.File.Create(path);
        await file.WriteAsync(bytes, CancellationToken);
        file.Close();
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

    #endregion

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

    protected virtual async Task<string> GetTempPathAsync(string hash)
    {
        var storagePath = await GetStoragePathAsync();
        return Path.Combine(Directory.GetCurrentDirectory(), storagePath, "temp", hash);
    }

    protected virtual Task<string> GetThumbnailFileNameAsync(string hash)
    {
        return Task.FromResult($"{hash}_thumbnail.png");
    }


    public virtual async Task SetPermissionsAsync(File entity, List<FilePermission> permissions)
    {
        var currentPermissions = await PermissionManager.GetListAsync(entity.Id, CancellationToken);

        var currentPermissionIds = new HashSet<Guid>(currentPermissions.Select(m => m.Id));
        var newPermissionIds = new HashSet<Guid>(permissions.Select(m => m.Id));

        // 找出需要删除的权限
        var removePermissions = currentPermissions.Where(m => !newPermissionIds.Contains(m.Id)).ToList();
        await PermissionManager.DeleteManyAsync(removePermissions, CancellationToken);

        // 找出需要新增的权限
        var insertPermissions = permissions.Where(m => !currentPermissionIds.Contains(m.Id)).ToList();
        await PermissionManager.InsertManyAsync(insertPermissions, CancellationToken);

        // 找出需要更新的权限
        var updatePermissions = permissions.Where(m => currentPermissionIds.Contains(m.Id)).ToList();
        await PermissionManager.UpdateManyAsync(updatePermissions, CancellationToken);
    }

    public virtual async Task<bool> CadReadAsync(File entity, Guid userId)
    {
        return await CheckPermissionAsync(entity, userId, PermissionManager.CanReadAsync);
    }

    public virtual async Task<bool> CadWriteAsync(File entity, Guid userId)
    {
        return await CheckPermissionAsync(entity, userId, PermissionManager.CanWriteAsync);
    }

    public virtual async Task<bool> CadDeleteAsync(File entity, Guid userId)
    {
        return await CheckPermissionAsync(entity, userId, PermissionManager.CanDeleteAsync);
    }

    public virtual async Task<bool> CheckPermissionAsync(File entity, Guid userId,
        Func<Guid, IList<string>, Expression<Func<FilePermission, bool>>, System.Threading.CancellationToken,
                Task<bool>>
            permissionCheckFunc)
    {
        Expression<Func<FilePermission, bool>> subPredicate = m =>
            m.ProviderName == FolderConsts.AuthorizationUserProviderName;

        var roles = await UserManager.GetRolesAsync(await UserManager.GetByIdAsync(userId));

        //判断文件夹是否存在认证用户权限
        subPredicate = subPredicate.OrIfNotTrue(m =>
            m.ProviderName == FolderConsts.AuthorizationUserProviderName);

        //判断是否包含用户
        subPredicate.OrIfNotTrue(m =>
            m.ProviderName == FolderConsts.UserProviderName && m.ProviderKey == userId.ToString());
        return await permissionCheckFunc(entity.Id, roles, subPredicate, CancellationToken);
    }

    public virtual async Task<string> GetStoragePathAsync()
    {
        return await SettingManager.GetOrNullForCurrentTenantAsync(FileManagementSettings.StoragePath);
    }
}