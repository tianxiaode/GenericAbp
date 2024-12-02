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
    ICancellationTokenProvider cancellationTokenProvider,
    IResourceConfigurationRepository configurationRepository) : ResourceManager(repository, treeCodeGenerator,
    localizer, cache, resourcePermissionManager, settingManager, distributedEventBus, cancellationTokenProvider,
    configurationRepository)
{
    public virtual async Task<Resource> GetUserFolderAsync(Guid id, ResourceQueryOptions? options = null)
    {
        var root = await GetUsersRootFolderAsync();
        options ??= new ResourceQueryOptions(false, includeConfiguration: true);
        var resource =
            await Repository.GetAsync(id, root.Id, options, CancellationToken);
        if (resource == null)
        {
            throw new EntityNotFoundBusinessException(Localizer["Folder"], id);
        }

        return resource;
    }

    public virtual async Task<Tuple<long, List<Resource>>> GetListAsync(
        ResourceSearchAndPagedAndSortedParams search, Guid? ownerId = null)
    {
        var root = await GetUsersRootFolderAsync();
        search.Sorting ??= $"{nameof(Resource.Name)}";
        var predicate = await Repository.BuildQueryExpressionAsync(root.Id, search);
        if (ownerId.HasValue)
        {
            predicate = predicate.And(m => m.Name.Contains(ownerId.Value.ToString()));
        }

        var count = await Repository.GetCountAsync(predicate, CancellationToken);
        var resources =
            await Repository.GetListAsync(predicate, search,
                new ResourceQueryOptions(false, includeConfiguration: true), CancellationToken);
        return Tuple.Create(count, resources);
    }

    public virtual async Task<Resource> CreateAsync(Guid userId, long quota, long maxFileSize, string allowFileTypes,
        bool isEnabled,
        Guid? tenantId = null)
    {
        var entity = await CreateRootFolderAsync(ResourceType.UserRootFolder,
            new FolderSetting(quota, maxFileSize, allowFileTypes), tenantId, userId, isEnabled);
        return entity;
    }

    public virtual async Task<Resource> UpdateAsync(Guid id, long storageQuota, long maxFileSize, string allowFileTypes,
        bool isEnabled, Guid? tenantId = null)
    {
        var entity = await GetUserFolderAsync(id, new ResourceQueryOptions(false, includeConfiguration: true));
        await CreateOrUpdateConfigurationAsync(entity, allowFileTypes, storageQuota, maxFileSize);

        entity.SetIsEnabled(isEnabled);
        await UpdateAsync(entity);
        return entity;
    }
}