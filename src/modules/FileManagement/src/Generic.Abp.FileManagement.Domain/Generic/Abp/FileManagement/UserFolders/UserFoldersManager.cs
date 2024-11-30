using System;
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
    IResourceConfigurationRepository resourceConfigurationRepository) : ResourceManager(repository, treeCodeGenerator,
    localizer, cache, resourcePermissionManager, settingManager, distributedEventBus, cancellationTokenProvider,
    resourceConfigurationRepository)
{
    public virtual async Task<Resource> GetAsync(Guid id, ResourceQueryOptions? options = null)
    {
        var root = await GetUsersRootFolderAsync();
        options ??= new ResourceQueryOptions(false, false, false, false, true);
        var resource =
            await Repository.GetAsync(id, root.Id, options, CancellationToken);
        if (resource == null)
        {
            throw new EntityNotFoundBusinessException(Localizer["Folder"], id);
        }

        return resource;
    }
}