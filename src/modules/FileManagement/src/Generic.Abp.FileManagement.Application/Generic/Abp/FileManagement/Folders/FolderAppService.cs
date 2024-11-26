using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.FileManagement.Dtos;
using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Files;
using Generic.Abp.FileManagement.Folders.Dtos;
using Generic.Abp.FileManagement.Permissions;
using Generic.Abp.FileManagement.Resources;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Generic.Abp.FileManagement.Folders;

[RemoteService(false)]
public class FolderAppService(
    ResourceManager resourceManager,
    IResourceRepository repository,
    FileInfoBaseManager fileInfoBaseManager)
    : FileManagementAppService, IFolderAppService
{
    protected ResourceManager ResourceManager { get; } = resourceManager;
    protected IResourceRepository Repository { get; } = repository;
    protected FileInfoBaseManager FileManager { get; } = fileInfoBaseManager;

    [Authorize(FileManagementPermissions.Resources.Default)]
    public virtual async Task<ListResultDto<FolderDto>> GetRootFoldersAsync()
    {
        var folderDtos = new List<FolderDto>();
        var publicRoot = await ResourceManager.GetPublicRootFolderAsync();
        folderDtos.Add(new FolderDto(publicRoot.Id, publicRoot.Code, L[publicRoot.Name]));

        var usersRoot = await ResourceManager.GetUsersRootFolderAsync();
        folderDtos.Add(new FolderDto(usersRoot.Id, usersRoot.Code, L[usersRoot.Name]));

        var sharedRoot = await ResourceManager.GetSharedRootFolderAsync();
        folderDtos.Add(new FolderDto(sharedRoot.Id, sharedRoot.Code, L[sharedRoot.Name]));
        return new ListResultDto<FolderDto>(folderDtos);
    }

    [Authorize(FileManagementPermissions.Resources.Default)]
    public virtual async Task<FolderDto> GetAsync(Guid id)
    {
        var folder = await Repository.GetAsync(id, false);
        return ObjectMapper.Map<Resource, FolderDto>(folder);
    }

    [Authorize(FileManagementPermissions.Resources.Default)]
    public virtual async Task<ListResultDto<FolderDto>> GetListAsync(FolderGetListInput input)
    {
        List<Resource> list = [];
        if (!string.IsNullOrEmpty(input.Filter))
        {
            list = await ResourceManager.GetSearchListAsync(m => m.Name.Contains(input.Filter));
        }
        else if (input.FolderId.HasValue)
        {
            list = await Repository.GetListAsync(m => m.ParentId == input.FolderId.Value);
        }

        return new ListResultDto<FolderDto>(ObjectMapper.Map<List<Resource>, List<FolderDto>>(list));
    }

    [Authorize(FileManagementPermissions.Resources.Create)]
    public async Task<FolderDto> CreateAsync(FolderCreateDto input)
    {
        throw new NotImplementedException();
        // var folder = new Resource(GuidGenerator.Create(), input.ParentId, input.Name, false, CurrentTenant.Id);
        // await FolderManager.CreateAsync(folder);
        // await UpdateByInputAsync(folder, input);
        // return ObjectMapper.Map<Folder, FolderDto>(folder);
    }

    [Authorize(FileManagementPermissions.Resources.Update)]
    public async Task<FolderDto> UpdateAsync(Guid id, FolderUpdateDto input)
    {
        throw new NotImplementedException();
        // var folder = await Repository.GetAsync(id, true);
        // if (folder.Name != input.Name)
        // {
        //     folder.Rename(input.Name);
        // }
        //
        // await UpdateByInputAsync(folder, input);
        // await Repository.UpdateAsync(folder);
        // return ObjectMapper.Map<Folder, FolderDto>(folder);
    }

    [Authorize(FileManagementPermissions.Resources.Update)]
    [UnitOfWork(true)]
    public virtual async Task MoveAsync(Guid id, Guid? parentId)
    {
        throw new NotImplementedException();
        // var entity = await Repository.GetAsync(id);
        // await CheckIsStaticAsync(entity);
        // await FolderManager.CheckMoveOrCopyAsync(entity, parentId);
        // await FolderManager.MoveAsync(entity, parentId);
    }

    [Authorize(FileManagementPermissions.Resources.Update)]
    [UnitOfWork(true)]
    public virtual async Task CopyAsync(Guid id, Guid? parentId)
    {
        throw new NotImplementedException();
        // await FolderManager.CheckMoveOrCopyAsync(await Repository.GetAsync(id), parentId);
        // await FolderManager.CopyAsync(id, parentId);
    }

    [Authorize(FileManagementPermissions.Resources.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
        // var entity = await FolderManager.GetAsync(id);
        // await CheckIsStaticAsync(entity);
        // await FolderManager.DeleteAsync(entity);
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
    public virtual async Task<FolderPermissionDto> GetFolderPermissionsAsync(Guid id)
    {
        throw new NotImplementedException();
        // var entity = await ResourceManager.GetAsync(id);
        // var permissions = await ResourceManager.GetPermissionsAsync(entity);
        // var dto = new FolderPermissionDto(entity.IsInheritPermissions)
        // {
        //     Permissions = ObjectMapper.Map<List<FolderPermission>, List<PermissionDto>>(permissions)
        // };
        // return dto;
    }

    [Authorize(FileManagementPermissions.Resources.ManagePermissions)]
    public virtual async Task UpdateFolderPermissionsAsync(Guid id, FolderPermissionUpdateDto input)
    {
        throw new NotImplementedException();
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

    protected virtual Task UpdateByInputAsync(Resource folder, FolderCreateOrUpdateDto input)
    {
        folder.SetQuota(input.StorageQuota);
        folder.SetUsedSize(input.UsedStorage);
        folder.SetMaxFileSize(input.MaxFileSize);
        folder.SetAllowedFileTypes(input.AllowedFileTypes);

        //folder.ChangeInheritPermissions(input.IsInheritPermissions);
        return Task.CompletedTask;
    }

    protected virtual Task CheckIsStaticAsync(Resource entity)
    {
        if (entity.IsStatic)
        {
            throw new StaticEntityCanNotBeUpdatedOrDeletedBusinessException(L["Folder"], entity.Name);
        }

        return Task.CompletedTask;
    }
}