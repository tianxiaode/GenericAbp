using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.Files;
using Generic.Abp.FileManagement.Folders.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;

namespace Generic.Abp.FileManagement.Folders;

[Authorize]
public class PersonalFolderAppService(
    FolderManager folderManager,
    IFolderRepository repository,
    FileManager fileManager)
    : FileManagementAppService, IPersonalFolderAppService
{
    protected FolderManager FolderManager { get; } = folderManager;
    protected IFolderRepository Repository { get; } = repository;
    protected FileManager FileManager { get; } = fileManager;

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

    public virtual async Task<FolderDto> GetAsync(Guid id)
    {
        var folder = await CheckFolderPermission(id, canRead: true);
        return ObjectMapper.Map<Folder, FolderDto>(folder);
    }

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