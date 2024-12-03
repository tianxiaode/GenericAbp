using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.Extensions.Trees;
using Generic.Abp.FileManagement.Localization;
using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.Settings;
using Microsoft.Extensions.Localization;
using Volo.Abp.Caching;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Threading;

namespace Generic.Abp.FileManagement.UserFolders;

public class UserFoldersManager(
    IResourceRepository repository,
    ITreeCodeGenerator<Resource> treeCodeGenerator,
    IStringLocalizer<FileManagementResource> localizer,
    IDistributedCache<ResourceCacheItem, string> cache,
    ResourcePermissionManager resourcePermissionManager,
    FileManagementSettingManager settingManager,
    IDistributedEventBus distributedEventBus,
    ICancellationTokenProvider cancellationTokenProvider) : ResourceManager(repository, treeCodeGenerator,
    localizer, cache, resourcePermissionManager, settingManager, distributedEventBus, cancellationTokenProvider)
{
    public virtual async Task<Resource> GetUserFolderAsync(Guid id, ResourceQueryOptions? options = null)
    {
        var root = await GetUsersRootFolderAsync();
        options ??= new ResourceQueryOptions(false);
        var resource =
            await Repository.GetAsync(id, root.Id, options, CancellationToken);
        if (resource == null)
        {
            throw new EntityNotFoundBusinessException(L["Folder"], id);
        }

        return resource;
    }

    public virtual async Task<(long, List<Resource>)> GetListAsync(
        ResourceQueryOptions options,
        ResourceSearchParams search, Guid? ownerId = null)
    {
        var root = await GetUsersRootFolderAsync();
        search.OwnerId = ownerId;
        var predicate = await Repository.BuildQueryExpressionAsync(root.Id, search);
        options.IncludeParent = false;
        var count = await Repository.GetCountAsync(predicate, CancellationToken);
        var resources =
            await Repository.GetListAsync(predicate, search, options, CancellationToken);
        return (count, resources);
    }

    public virtual async Task<Resource> CreateAsync(Guid userId, long quota, long maxFileSize, string allowFileTypes,
        bool isAccessible,
        Guid? tenantId = null)
    {
        var entity = await CreateRootFolderAsync(ResourceType.UserRootFolder,
            new FolderSetting(quota, maxFileSize, allowFileTypes), tenantId, userId, isAccessible);
        return entity;
    }

    public virtual async Task<Resource> UpdateAsync(Guid id, long storageQuota, long maxFileSize, string allowFileTypes,
        bool isAccessible, Guid? tenantId = null)
    {
        var entity = await GetUserFolderAsync(id, new ResourceQueryOptions(false));
        entity.SetAllowedFileTypes(allowFileTypes);
        entity.SetStorageQuota(storageQuota);
        entity.SetMaxFileSize(maxFileSize);
        entity.SetIsAccessible(isAccessible);
        await UpdateAsync(entity);
        return entity;
    }
}