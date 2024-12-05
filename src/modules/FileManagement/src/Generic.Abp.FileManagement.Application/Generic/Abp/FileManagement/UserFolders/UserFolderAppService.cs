using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.FileManagement.Permissions;
using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.Resources.Dtos;
using Generic.Abp.FileManagement.UserFolders.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Generic.Abp.FileManagement.UserFolders;

[RemoteService(false)]
public class UserFolderAppService(
    ResourceManager resourceManager,
    IdentityUserManager identityUserManager,
    IIdentityUserRepository userRepository)
    : FileManagementAppService, IUserFolderAppService
{
    protected ResourceManager ResourceManager { get; } = resourceManager;
    protected IdentityUserManager IdentityUserManager { get; } = identityUserManager;
    protected IIdentityUserRepository UserRepository { get; } = userRepository;

    [Authorize(FileManagementPermissions.UserFolders.Default)]
    public virtual async Task<ResourceBaseDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<Resource, ResourceBaseDto>(await ResourceManager.GetOrCreateUserRootFolderAsync(id));
    }

    [Authorize(FileManagementPermissions.UserFolders.Default)]
    public virtual async Task<PagedResultDto<ResourceBaseDto>> GetListAsync(UserFolderGetListInput input)
    {
        var queryParams = ObjectMapper.Map<UserFolderGetListInput, ResourceQueryParams>(input);
        var rootFolder = await ResourceManager.GetUsersRootFolderAsync();
        queryParams.ParentId = rootFolder.Id;
        var predicate = await ResourceManager.BuildPredicateExpressionAsync(queryParams);
        var count = await ResourceManager.GetCountAsync(predicate);
        var list = await ResourceManager.GetPagedListAsync(predicate, queryParams);
        return new PagedResultDto<ResourceBaseDto>(count,
            ObjectMapper.Map<List<Resource>, List<ResourceBaseDto>>(list));
    }

    [Authorize(FileManagementPermissions.UserFolders.Default)]
    public virtual async Task<PagedResultDto<UserDto>> GetUsersAsync(UserGetListInput input)
    {
        var count = await UserRepository.GetCountAsync(input.Filter);
        var users = await UserRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount,
            input.Filter);
        return new PagedResultDto<UserDto>(count, ObjectMapper.Map<List<IdentityUser>, List<UserDto>>(users));
    }


    [Authorize(FileManagementPermissions.UserFolders.Create)]
    public virtual async Task<ResourceBaseDto> CreateAsync(UserFolderCreateDto input)
    {
        var owner = await IdentityUserManager.GetByIdAsync(input.OwnerId);
        if (owner == null)
        {
            throw new EntityNotFoundBusinessException(L["User"], input.OwnerId);
        }

        var entity = await ResourceManager.GetOrCreateUserRootFolderAsync(owner.Id, CurrentTenant.Id);
        return ObjectMapper.Map<Resource, ResourceBaseDto>(entity);
    }

    [Authorize(FileManagementPermissions.UserFolders.Update)]
    public virtual async Task<ResourceBaseDto> UpdateAsync(Guid id, UserFolderUpdateDto input)
    {
        var entity = await ResourceManager.GetOrCreateUserRootFolderAsync(id, CurrentTenant.Id);
        if (entity == null)
        {
            throw new EntityNotFoundBusinessException(L["UserFolder"], id);
        }

        entity.SetAllowedFileTypes(input.AllowedFileTypes);
        entity.SetMaxFileSize(input.MaxFileSize);
        entity.SetStorageQuota(input.StorageQuota);
        entity.SetIsAccessible(input.IsAccessible);
        await ResourceManager.UpdateAsync(entity);
        return ObjectMapper.Map<Resource, ResourceBaseDto>(entity);
    }

    [Authorize(FileManagementPermissions.UserFolders.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await ResourceManager.DeleteAsync(id);
    }
}