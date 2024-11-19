using Generic.Abp.Extensions.MimeDetective;
using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Folders;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Extensions;
using Generic.Abp.FileManagement.Exceptions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;

namespace Generic.Abp.FileManagement.Files;

[Authorize]
public class FileAppService(
    FileInfoBaseManager fileInfoBaseManager,
    FolderManager folderManager,
    FileManager fileManager)
    : FileManagementAppService, IFileAppService
{
    protected FileInfoBaseManager FileInfoBaseManager { get; } = fileInfoBaseManager;
    protected FolderManager FolderManager { get; } = folderManager;
    protected FileManager FileManager { get; } = fileManager;

    protected virtual async Task<FileDto> GetAsync(Guid folderId, Guid id)
    {
        await CheckFolderPermission(folderId, canRead: true);
        var entity = await FileManager.GetAsync(id);
        await CheckFilePermission(entity);
        return ObjectMapper.Map<File, FileDto>(entity);
    }

    public virtual async Task<ICheckFileResult> CheckFileAsync(FileCheckInput input)
    {
        var folder = await CheckFolderPermission(input.FolderId, canWrite: true);
        if (input.Size > folder.MaxFileSize)
        {
            throw new FileSizeOutOfRangeBusinessException(folder.MaxFileSize, input.Size);
        }

        if (input.Size > folder.StorageQuota - folder.UsedStorage)
        {
            throw new InsufficientStorageSpaceBusinessException(input.Size, folder.UsedStorage, folder.StorageQuota);
        }

        return await FileInfoBaseManager.CheckAsync(input.Hash, input.Size, input.ChunkSize);
    }

    public virtual async Task UploadChunkAsync(FileUploadChunkInput input)
    {
        var folder = await CheckFolderPermission(input.FolderId, canWrite: true);
        await FileInfoBaseManager.UploadChunkAsync(input.Hash, input.ChunkBytes, input.Index);
    }

    public virtual async Task<FileDto> MergeAsync(FileMergeInput input)
    {
        var folder = await CheckFolderPermission(input.FolderId, canWrite: true);
        ;
        var fileInfoBase = await FileInfoBaseManager.MergeAsync(input.Hash, input.TotalChunks,
            await folder.AllowedFileTypes.GetFileTypesAsync(), folder.MaxFileSize,
            folder.StorageQuota, folder.UsedStorage);
        var entity = new File(GuidGenerator.Create(), folder.Id, input.Hash, CurrentTenant.Id);
        var filename = await FileManager.GetFileNameAsync(folder.Id, input.Filename);
        entity.Rename(filename);
        entity.SetFileInfoBase(fileInfoBase);
        await FileManager.CreateAsync(entity);
        return ObjectMapper.Map<File, FileDto>(entity);
    }

    protected virtual async Task CheckFilePermission(File entity)
    {
        if (!CurrentUser.Id.HasValue)
        {
            throw new AbpAuthorizationException(L["AccessDenied"]);
        }

        if (entity.IsInheritPermissions)
        {
            return;
        }

        if (await FileManager.CadReadAsync(entity, CurrentUser.Id.Value))
        {
            return;
        }

        throw new AbpAuthorizationException(L["AccessDenied"]);
    }

    protected virtual async Task<Folder> CheckFolderPermission(Guid id, bool canRead = false, bool canWrite = false,
        bool canDelete = false)
    {
        var folder = await FolderManager.GetAsync(id);
        if (!CurrentUser.Id.HasValue)
        {
            throw new AbpAuthorizationException(L["AccessDenied"]);
        }

        if (await FolderManager.IsOwnerAsync(folder, CurrentUser.Id.Value))
        {
            return folder;
        }

        if (canWrite && await FolderManager.IsRooFolderAsync(folder))
        {
            throw new AbpAuthorizationException(L["AccessDenied"]);
        }

        var can =
            canRead ? await FolderManager.CadWriteAsync(folder, CurrentUser.Id.Value) :
            canWrite ? await FolderManager.CadWriteAsync(folder, CurrentUser.Id.Value) :
            canDelete && await FolderManager.CadDeleteAsync(folder, CurrentUser.Id.Value);
        if (can)
        {
            return folder;
        }

        throw new AbpAuthorizationException(L["AccessDenied"]);
    }
}