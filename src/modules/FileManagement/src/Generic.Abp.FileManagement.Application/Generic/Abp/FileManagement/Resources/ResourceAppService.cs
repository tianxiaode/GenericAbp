using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Permissions;
using Generic.Abp.FileManagement.Resources.Dtos;
using Generic.Abp.FileManagement.Resources.Dtos.Folders;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;

namespace Generic.Abp.FileManagement.Resources;

[RemoteService(false)]
public class ResourceAppService(
    ResourceManager resourceManager,
    IResourceRepository repository,
    FileInfoBaseManager fileInfoBaseManager)
    : FileManagementAppService, IResourceAppService
{
    protected ResourceManager ResourceManager { get; } = resourceManager;
    protected IResourceRepository Repository { get; } = repository;
    protected FileInfoBaseManager FileManager { get; } = fileInfoBaseManager;

    [Authorize(FileManagementPermissions.Resources.Default)]
    public virtual async Task<ListResultDto<ResourceBaseDto>> GetRootFoldersAsync()
    {
        var folderDtos = new List<ResourceBaseDto>();
        var publicRoot = await ResourceManager.GetPublicRootFolderAsync();
        folderDtos.Add(new ResourceBaseDto(publicRoot.Id, publicRoot.Code, L[publicRoot.Name]));

        var sharedRoot = await ResourceManager.GetSharedRootFolderAsync();
        folderDtos.Add(new ResourceBaseDto(sharedRoot.Id, sharedRoot.Code, L[sharedRoot.Name]));
        return new ListResultDto<ResourceBaseDto>(folderDtos);
    }

    [Authorize(FileManagementPermissions.Resources.Default)]
    public virtual async Task<ResourceBaseDto> GetAsync(Guid id)
    {
        var folder = await Repository.GetAsync(id, false);
        return ObjectMapper.Map<Resource, ResourceBaseDto>(folder);
    }

    [Authorize(FileManagementPermissions.Resources.Default)]
    public virtual async Task<ListResultDto<ResourceBaseDto>> GetFolderListAsync(Guid parentId,
        ResourceGetListInput input)
    {
        if (!CurrentUser.Id.HasValue)
        {
            throw new AbpException(L["AccessDenied"]);
        }

        var publicRoot = await ResourceManager.GetPublicRootFolderAsync();
        var sharedRoot = await ResourceManager.GetSharedRootFolderAsync();
        if (parentId != publicRoot.Id && parentId != sharedRoot.Id)
        {
            throw new EntityNotFoundBusinessException(L["Folder"], parentId);
        }

        var queryParams = ObjectMapper.Map<ResourceGetListInput, ResourceQueryParams>(input);
        queryParams.ParentId = parentId;
        queryParams.ResourceType = ResourceType.Folder;

        var list = await ResourceManager.GetListByPermissionAsync(queryParams, CurrentUser.Id.Value,
            ResourcePermissionType.CanRead);

        return new ListResultDto<ResourceBaseDto>(ObjectMapper.Map<List<Resource>, List<ResourceBaseDto>>(list));
    }

    [Authorize(FileManagementPermissions.Resources.Default)]
    public async Task<ResourceBaseDto> CreateAsync(Guid parentId,FolderCreateDto input)
    {
        if (!await ResourceManager.IsSharedFolderAsync(parentId))
        {

        }

        var entity = await ResourceManager.CreateFolderAsync(input.Name, input.ParentId, input.AllowedFileTypes,
            input.Quota, input.MaxFileSize, CurrentTenant.Id);
        return ObjectMapper.Map<Resource, ResourceBaseDto>(entity);
    }

    [Authorize(FileManagementPermissions.Resources.Update)]
    public async Task<ResourceBaseDto> UpdateAsync(Guid id, FolderUpdateDto input)
    {
        var entity =
            await ResourceManager.UpdateFolderAsync(id, input.Name, input.AllowedFileTypes, input.Quota,
                input.MaxFileSize);
        return ObjectMapper.Map<Resource, ResourceBaseDto>(entity);
    }

    [Authorize(FileManagementPermissions.Resources.Update)]
    public virtual async Task MoveAsync(Guid id, Guid parentId)
    {
        await ResourceManager.MoveFolderAsync(id, parentId);
    }

    [Authorize(FileManagementPermissions.Resources.Update)]
    public virtual async Task CopyAsync(Guid id, Guid parentId)
    {
        await ResourceManager.CopyFolderAsync(id, parentId);
    }

    [Authorize(FileManagementPermissions.Resources.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await ResourceManager.DeleteFolderAsync(id);
    }

    #region Files

    [Authorize(FileManagementPermissions.Resources.Default)]
    public virtual async Task<FileDto> GetFileAsync(Guid id)
    {
        throw new NotImplementedException();
        // var file = await FileRepository.GetAsync(id);
        // return ObjectMapper.Map<File, FileDto>(file);
    }

    [Authorize(FileManagementPermissions.Resources.Default)]
    public virtual async Task<PagedResultDto<FileDto>> GetFileListAsync(FileGetListInput input)
    {
        throw new NotImplementedException();
        // if (string.IsNullOrEmpty(input.Filter) && !input.FolderId.HasValue)
        // {
        //     throw new UserFriendlyException(L["NoFilterAndFolderSelected"]);
        // }
        //
        // var predicate = await FileRepository.BuildPredicate(input.FolderId, input.Filter, input.StartTime,
        //     input.EndTime,
        //     input.FileTypes, input.MinSize, input.MaxSize);
        // var count = await FileRepository.LongCountAsync(predicate);
        // var list = await FileRepository.GetListAsync(predicate, input.Sorting, input.MaxResultCount,
        //     input.SkipCount);
        // return new PagedResultDto<FileDto>(count, ObjectMapper.Map<List<File>, List<FileDto>>(list));
    }

    [Authorize(FileManagementPermissions.Resources.Update)]
    public virtual async Task<FileDto> UpdateFileAsync(Guid id, FileUpdateDto input)
    {
        throw new NotImplementedException();
        // var entity = await FileRepository.GetAsync(id);
        // await CheckIsStaticAsync(entity.Folder);
        // entity.Rename(input.Filename);
        // entity.SetDescription(input.Description);
        // await FileRepository.UpdateAsync(entity);
        // return ObjectMapper.Map<File, FileDto>(entity);
    }

    [Authorize(FileManagementPermissions.Resources.Delete)]
    public virtual async Task DeleteFileAsync(Guid id)
    {
        throw new NotImplementedException();
        // var entity = await FileRepository.GetAsync(id);
        // await CheckIsStaticAsync(entity.Folder);
        // await FileRepository.DeleteAsync(entity);
    }

    [Authorize(FileManagementPermissions.Resources.ManagePermissions)]
    public virtual async Task<FilePermissionDto> GetFilePermissionsAsync(Guid id)
    {
        throw new NotImplementedException();
        // var entity = await FileManager.GetAsync(id);
        // var permissions = await FileManager.GetPermissionsAsync(id);
        // var dto = new FilePermissionDto(entity.IsInheritPermissions)
        // {
        //     Permissions = ObjectMapper.Map<List<FilePermission>, List<PermissionDto>>(permissions)
        // };
        // return dto;
    }

    [Authorize(FileManagementPermissions.Resources.ManagePermissions)]
    public virtual async Task UpdateFilePermissionsAsync(Guid id, FilePermissionUpdateDto input)
    {
        throw new NotImplementedException();
        // var entity = await FileManager.GetAsync(id);
        // if (input.IsInheritPermissions)
        // {
        //     entity.ChangeInheritPermissions(true);
        // }
        // else
        // {
        //     entity.ChangeInheritPermissions(false);
        //     var permissions = input.Permissions.Select(m =>
        //         new FilePermission(m.Id ?? GuidGenerator.Create(), id, m.ProviderName, m.ProviderKey,
        //             m.CanRead, m.CanWrite, m.CanDelete, CurrentTenant.Id)).ToList();
        //     await FileManager.SetPermissionsAsync(entity, permissions);
        // }
    }

    #endregion

    #region Permissons

    [Authorize(FileManagementPermissions.Resources.ManagePermissions)]
    public virtual async Task<ListResultDto<ResourcePermissionDto>> GetFolderPermissionsAsync(Guid id)
    {
        var entity = await ResourceManager.GetAsync(id);
        if (await ResourceManager.IsVirtualFolderAsync(entity) || entity.Type != ResourceType.Folder)
        {
            throw new EntityNotFoundBusinessException(L["Folder"], id);
        }

        var permissions = await ResourceManager.GetPermissionsAsync(entity);
        return new ListResultDto<ResourcePermissionDto>(
            ObjectMapper.Map<List<ResourcePermission>, List<ResourcePermissionDto>>(permissions));
    }

    [Authorize(FileManagementPermissions.Resources.ManagePermissions)]
    public virtual async Task UpdateFolderPermissionsAsync(Guid id, ResourcePermissionsCreateOrUpdateDto input)
    {
        var entity = await ResourceManager.GetAsync(id);
        if (await ResourceManager.IsVirtualFolderAsync(entity) || entity.Type != ResourceType.Folder)
        {
            throw new EntityNotFoundBusinessException(L["Folder"], id);
        }

        // var entity = await FolderManager.GetAsync(id);
        // if (input.IsInheritPermissions)
        // {
        //     entity.ChangeInheritPermissions(true);
        // }
        // else
        // {
        //     entity.ChangeInheritPermissions(false);
        //     var permissions = input.Permissions.Select(m =>
        //         new FolderPermission(m.Id ?? GuidGenerator.Create(), id, m.ProviderName, m.ProviderKey,
        //             m.CanRead, m.CanWrite, m.CanDelete, CurrentTenant.Id)).ToList();
        //     await FolderManager.SetPermissionsAsync(entity, permissions);
        // }
    }

    #endregion
}