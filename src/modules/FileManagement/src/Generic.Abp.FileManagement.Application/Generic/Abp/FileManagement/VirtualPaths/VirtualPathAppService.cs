using Generic.Abp.Extensions.RemoteContents;
using Generic.Abp.FileManagement.Dtos;
using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Permissions;
using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.VirtualPaths.Dtos;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.FileManagement.Exceptions;
using Generic.Abp.FileManagement.Resources.Dtos;
using Generic.Abp.VirtualPaths;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Resource = Generic.Abp.FileManagement.Resources.Resource;
using System.IO;


namespace Generic.Abp.FileManagement.VirtualPaths;

[RemoteService(false)]
public class VirtualPathAppService(
    VirtualPathManager virtualPathManager,
    FileInfoBaseManager fileInfoBaseManager)
    : FileManagementAppService, IVirtualPathAppService
{
    protected VirtualPathManager VirtualPathManager { get; } = virtualPathManager;
    protected FileInfoBaseManager FileInfoBaseManager { get; } = fileInfoBaseManager;

    #region 文件相关

    

    [AllowAnonymous]
    public virtual async Task<IRemoteContent> GetFileAsync(string path, string hash, GetFileDto input)
    {
        var entity = await VirtualPathManager.FindVirtualPathAsync(path);


        //是否有权限
        await CheckReadPermissionAsync(entity);

        var file = await GetAndValidateFileAssociationAsync(entity, hash);

        return await FileInfoBaseManager.GetFileAsync(file, input.ChunkSize, input.Index);
    }

    [Authorize]
    public virtual async Task<ICheckFileResult> CheckFileAsync(string path, FileCheckInput input)
    {
        var entity =
            await VirtualPathManager.FindVirtualPathAsync(path,
                new ResourceQueryOptions(false, includeConfiguration: true));
        await CheckWritePermissionAsync(entity);
        if (entity.Configuration == null)
        {
            Logger.LogError("The virtual path {name} does not have a configuration", entity.Name);
            throw new AbpException("The virtual path does not have a configuration");
        }

        if (input.Size > entity.Configuration.MaxFileSize)
        {
            throw new FileSizeOutOfRangeBusinessException(entity.Configuration.MaxFileSize, input.Size);
        }

        if (entity.Configuration.StorageQuota != 0 &&
            input.Size > entity.Configuration.StorageQuota - entity.Configuration.UsedStorage)
        {
            throw new InsufficientStorageSpaceBusinessException(input.Size, entity.Configuration.UsedStorage,
                entity.Configuration.StorageQuota);
        }

        return await FileInfoBaseManager.CheckAsync(input.Hash, input.Size, input.ChunkSize);
    }

    [Authorize]
    public virtual async Task UploadChunkAsync(string path, FileUploadChunkInput input)
    {
        var entity =
            await VirtualPathManager.FindVirtualPathAsync(path,
                new ResourceQueryOptions(false, includeConfiguration: true));
        await CheckWritePermissionAsync(entity);
        if (entity.Configuration == null)
        {
            Logger.LogError("The virtual path {name} does not have a configuration", entity.Name);
            throw new AbpException("The virtual path does not have a configuration");
        }

        if (!entity.FolderId.HasValue)
        {
            throw new AbpException();
        }

        await FileInfoBaseManager.UploadChunkAsync(input.Hash, input.ChunkBytes, input.Index);
    }

    public virtual async Task<ResourceDto> MergeAsync(string path, FileMergeInput input)
    {
        var entity =
            await VirtualPathManager.FindVirtualPathAsync(path,
                new ResourceQueryOptions(false, true, includeConfiguration: true));
        await CheckWritePermissionAsync(entity);
        if (entity.Configuration == null)
        {
            Logger.LogError("The virtual path {name} does not have a configuration", entity.Name);
            throw new AbpException("The virtual path does not have a configuration");
        }

        if (!entity.FolderId.HasValue)
        {
            throw new AbpException();
        }

        var fileInfoBase = await FileInfoBaseManager.MergeAsync(input.Hash, input.TotalChunks,
            entity.Configuration.AllowedFileTypes, entity.Configuration.MaxFileSize,
            entity.Configuration.StorageQuota, entity.Configuration.UsedStorage);
        var fileResource = new Resource(GuidGenerator.Create(), input.Filename, ResourceType.File, false,
            CurrentTenant.Id);
        fileResource.MoveTo(entity.FolderId.Value);
        fileResource.SetFileInfoBase(fileInfoBase.Id);
        await VirtualPathManager.CreateAsync(fileResource);
        return ObjectMapper.Map<Resource, ResourceDto>(entity);
    }

    public virtual Task DeleteFileAsync(string path, Guid id)
    {

    }

    #endregion

    [Authorize(FileManagementPermissions.VirtualPaths.Default)]
    public virtual async Task<VirtualPathDto> GetAsync(Guid id)
    {
        var entity = await VirtualPathManager.GetVirtualPathAsync(id);

        return ObjectMapper.Map<Resource, VirtualPathDto>(entity);
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Default)]
    public virtual async Task<VirtualPathDto> FindByNameAsync(string name)
    {
        var entity = await VirtualPathManager.FindVirtualPathAsync(name);
        return ObjectMapper.Map<Resource, VirtualPathDto>(entity);
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Default)]
    public virtual async Task<PagedResultDto<ResourceDto>> GetListAsync(VirtualPathGetListInput input)
    {
        var (count, list) = await VirtualPathManager.GetVirtualPathsAsync(input.Filter, input.StartTime, input.EndTime,
            input.FileType, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<ResourceDto>(count, MapToResourceDtos(list));
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Create)]
    public virtual async Task<ResourceDto> CreateAsync(VirtualPathCreateDto input)
    {
        var entity = await VirtualPathManager.CreateVirtualPathAsync(input.Name, input.FolderId);
        return ObjectMapper.Map<Resource, ResourceDto>(entity);
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Update)]
    public virtual async Task<ResourceDto> UpdateAsync(Guid id, VirtualPathUpdateDto input)
    {
        var entity = await VirtualPathManager.UpdateVirtualPathAsync(id, input.Name, input.FolderId);
        return ObjectMapper.Map<Resource, ResourceDto>(entity);
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await VirtualPathManager.DeleteVirtualPathAsync(id);
    }

    #region Permission

    [Authorize(FileManagementPermissions.VirtualPaths.ManagePermissions)]
    public virtual async Task<ListResultDto<ResourcePermissionDto>> GetPermissionsAsync(Guid id)
    {
        var list = await VirtualPathManager.GetVirtualPathPermissionAsync(id);
        return new ListResultDto<ResourcePermissionDto>(
            ObjectMapper.Map<List<ResourcePermission>, List<ResourcePermissionDto>>(list));
    }

    [Authorize(FileManagementPermissions.VirtualPaths.ManagePermissions)]
    public virtual async Task UpdatePermissionAsync(Guid id,
        ResourcePermissionsCreateOrUpdateDto input)
    {
        var permissions = input.Permissions.Select(m =>
            new ResourcePermission(m.Id ?? GuidGenerator.Create(), id, m.ProviderName, m.ProviderKey,
                m.Permissions, CurrentTenant.Id)).ToList();
        await VirtualPathManager.UpdateVirtualPathPermissionAsync(id, permissions);
    }

    #endregion

    protected virtual async Task<FileInfoBase> GetAndValidateFileAssociationAsync(Resource entity, string hash)
    {
        // 校验文件夹关联
        if (entity.Folder == null)
        {
            throw new EntityNotFoundBusinessException(L["File"], hash);
        }

        // 校验文件是否存在
        var file = await FileInfoBaseManager.FindByHashAsync(hash);
        if (file == null)
        {
            throw new EntityNotFoundBusinessException(L["File"], hash);
        }

        // 校验文件是否存在于文件夹中
        if (!await VirtualPathManager.FileExistsAsync(entity.Folder.Code, file.Id))
        {
            throw new EntityNotFoundBusinessException(L["File"], hash);
        }

        // 返回文件实例
        return file;
    }

    protected virtual async Task CheckReadPermissionAsync(Resource resource)
    {
        var canRead = await VirtualPathManager.CanReadAsync(resource, CurrentUser.Id);
        if (!canRead)
        {
            throw new AbpAuthorizationException(L["AccessDenied"]);
        }
    }

    protected virtual async Task CheckWritePermissionAsync(Resource resource)
    {
        var canWrite = await VirtualPathManager.CanWriteAsync(resource, CurrentUser.Id);
        if (!canWrite)
        {
            throw new AbpAuthorizationException(L["AccessDenied"]);
        }
    }
}