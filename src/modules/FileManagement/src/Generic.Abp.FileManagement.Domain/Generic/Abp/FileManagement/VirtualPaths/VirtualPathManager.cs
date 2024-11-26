using Generic.Abp.Extensions.Trees;
using Generic.Abp.FileManagement.Localization;
using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.Settings;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Threading;

namespace Generic.Abp.FileManagement.VirtualPaths;

public class VirtualPathManager(
    IResourceRepository repository,
    ITreeCodeGenerator<Resource> treeCodeGenerator,
    IStringLocalizer<FileManagementResource> localizer,
    IDistributedCache<ResourceCacheItem, string> cache,
    ResourcePermissionManager resourcePermissionManager,
    FileManagementSettingManager settingManager,
    IDistributedEventBus distributedEventBus,
    ICancellationTokenProvider cancellationTokenProvider) : ResourceManagerBase(repository, treeCodeGenerator,
    localizer, cache, resourcePermissionManager, settingManager, distributedEventBus, cancellationTokenProvider)
{
    public virtual async Task<Resource> FindByPathAsync(string path)
    {
        var entity =
            await Repository.FindAsync(m => m.Name.ToLower() == path.ToLowerInvariant(), false, CancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(Resource), path);
        }

        return entity;
    }
}