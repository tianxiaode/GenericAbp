using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.FileManagement.Folders.Dtos;
using Generic.Abp.FileManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Uow;

namespace Generic.Abp.FileManagement.Folders;

[RemoteService(false)]
public class FolderAppService(FolderManager folderManager, IFolderRepository repository)
    : FileManagementAppService, IFolderAppService
{
    protected FolderManager FolderManager { get; } = folderManager;
    protected IFolderRepository Repository { get; } = repository;

    [Authorize(FileManagementPermissions.Folders.Default)]
    public virtual async Task<ListResultDto<FolderDto>> GetRootFoldersAsync()
    {
        var folderDtos = new List<FolderDto>();
        var publicRoot = await FolderManager.GetPublicRootFolderAsync();
        folderDtos.Add(new FolderDto(publicRoot.Id, publicRoot.Code, L[publicRoot.Name]));

        var usersRoot = await FolderManager.GetUsersRootFolderAsync();
        folderDtos.Add(new FolderDto(usersRoot.Id, usersRoot.Code, L[usersRoot.Name]));

        var sharedRoot = await FolderManager.GetSharedRootFolderAsync();
        folderDtos.Add(new FolderDto(sharedRoot.Id, sharedRoot.Code, L[sharedRoot.Name]));
        return new ListResultDto<FolderDto>(folderDtos);
    }

    [Authorize(FileManagementPermissions.Folders.Default)]
    public virtual async Task<FolderDto> GetAsync(Guid id)
    {
        var folder = await Repository.GetAsync(id, false);
        return ObjectMapper.Map<Folder, FolderDto>(folder);
    }

    [Authorize(FileManagementPermissions.Folders.Default)]
    public virtual async Task<ListResultDto<FolderDto>> GetListAsync(FolderGetListInput input)
    {
        List<Folder> list = [];
        if (!string.IsNullOrEmpty(input.Filter))
        {
            list = await FolderManager.GetSearchListAsync(m => m.Name.Contains(input.Filter));
        }
        else if (input.ParentId.HasValue)
        {
            list = await Repository.GetListAsync(m => m.ParentId == input.ParentId.Value);
        }

        return new ListResultDto<FolderDto>(ObjectMapper.Map<List<Folder>, List<FolderDto>>(list));
    }

    [Authorize(FileManagementPermissions.Folders.Create)]
    public async Task<FolderDto> CreateAsync(FolderCreateDto input)
    {
        var folder = new Folder(GuidGenerator.Create(), input.ParentId, input.Name, false, CurrentTenant.Id);
        await FolderManager.CreateAsync(folder);
        await UpdateByInputAsync(folder, input);
        return ObjectMapper.Map<Folder, FolderDto>(folder);
    }

    [Authorize(FileManagementPermissions.Folders.Update)]
    public async Task<FolderDto> UpdateAsync(Guid id, FolderUpdateDto input)
    {
        var folder = await Repository.GetAsync(id, true);
        if (folder.Name != input.Name)
        {
            folder.Rename(input.Name);
        }

        await UpdateByInputAsync(folder, input);
        await Repository.UpdateAsync(folder);
        return ObjectMapper.Map<Folder, FolderDto>(folder);
    }

    [Authorize(FileManagementPermissions.Folders.Update)]
    [UnitOfWork(true)]
    public virtual async Task MoveAsync(Guid id, Guid? parentId)
    {
        var entity = await Repository.GetAsync(id);
        await CheckIsStaticAsync(entity);
        await FolderManager.CheckMoveOrCopyAsync(entity, parentId);
        await FolderManager.MoveAsync(entity, parentId);
    }

    [Authorize(FileManagementPermissions.Folders.Update)]
    [UnitOfWork(true)]
    public virtual async Task CopyAsync(Guid id, Guid? parentId)
    {
        await FolderManager.CheckMoveOrCopyAsync(await Repository.GetAsync(id), parentId);
        await FolderManager.CopyAsync(id, parentId);
    }

    [Authorize(FileManagementPermissions.Folders.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        var entity = await FolderManager.GetAsync(id);
        await CheckIsStaticAsync(entity);
        await FolderManager.DeleteAsync(entity);
    }


    protected virtual Task UpdateByInputAsync(Folder folder, FolderCreateOrUpdateDto input)
    {
        folder.SetStorageQuota(input.StorageQuota);
        folder.SetUsedStorage(input.UsedStorage);
        folder.SetMaxFileSize(input.MaxFileSize);
        folder.SetAllowedFileTypes(input.AllowedFileTypes);
        ;
        folder.ChangeInheritPermissions(input.IsInheritPermissions);
        return Task.CompletedTask;
    }

    protected virtual Task CheckIsStaticAsync(Folder entity)
    {
        if (entity.IsStatic)
        {
            throw new StaticEntityCanNotBeUpdatedOrDeletedBusinessException(L["Folder"], entity.Name);
        }

        return Task.CompletedTask;
    }
}