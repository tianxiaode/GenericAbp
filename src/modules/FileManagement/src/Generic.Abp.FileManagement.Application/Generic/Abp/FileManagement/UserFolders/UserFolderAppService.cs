using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.Permissions;
using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.UserFolders.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Generic.Abp.FileManagement.UserFolders;

[RemoteService(false)]
public class UserFolderAppService(UserFoldersManager userFoldersManager, IdentityUserManager identityUserManager)
    : FileManagementAppService, IUserFolderAppService
{
    protected UserFoldersManager UserFoldersManager { get; } = userFoldersManager;
    protected IdentityUserManager IdentityUserManager { get; } = identityUserManager;

    [Authorize(FileManagementPermissions.UserFolders.Default)]
    public virtual async Task<UserFolderDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Resource, UserFolderDto>(await UserFoldersManager.GetUserFolderAsync(id));
    }

    [Authorize(FileManagementPermissions.UserFolders.Default)]
    public virtual async Task<PagedResultDto<UserFolderDto>> GetListAsync(UserFolderGetListInput input)
    {
        var (cout, list) = await UserFoldersManager.GetListAsync(
            ObjectMapper.Map<UserFolderGetListInput, ResourceSearchAndPagedAndSortedParams>(input), input.OwnerId);
        return new PagedResultDto<UserFolderDto>(cout, ObjectMapper.Map<List<Resource>, List<UserFolderDto>>(list));
    }

    [Authorize(FileManagementPermissions.UserFolders.Create)]
    public virtual async Task<UserFolderDto> CreateAsync(UserFolderCreateDto input)
    {
        var owner = await IdentityUserManager.GetByIdAsync(input.OwnerId);
        var entity = await UserFoldersManager.CreateAsync(owner.Id, input.StorageQuota, input.MaxFileSize,
            input.AllowedFileTypes, input.IsEnabled, CurrentTenant.Id);
        return ObjectMapper.Map<Resource, UserFolderDto>(entity);
    }

    [Authorize(FileManagementPermissions.UserFolders.Update)]
    public virtual async Task<UserFolderDto> UpdateAsync(Guid id, UserFolderUpdateDto input)
    {
        var entity = await UserFoldersManager.UpdateAsync(id, input.StorageQuota, input.MaxFileSize,
            input.AllowedFileTypes,
            input.IsEnabled);
        return ObjectMapper.Map<Resource, UserFolderDto>(entity);
    }

    [Authorize(FileManagementPermissions.UserFolders.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await UserFoldersManager.DeleteAsync(id);
    }
}