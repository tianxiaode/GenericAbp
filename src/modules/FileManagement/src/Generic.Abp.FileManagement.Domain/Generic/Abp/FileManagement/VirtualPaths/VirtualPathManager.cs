using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.Extensions.Trees;
using Generic.Abp.FileManagement.Localization;
using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.Settings;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Caching;
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
    ICancellationTokenProvider cancellationTokenProvider,
    IResourceConfigurationRepository configurationRepository) : ResourceManager(repository, treeCodeGenerator,
    localizer,
    cache, resourcePermissionManager, settingManager, distributedEventBus, cancellationTokenProvider,
    configurationRepository)
{
    public virtual async Task<Resource> GetVirtualPathAsync(Guid id, ResourceQueryOptions? options = null)
    {
        var root = await GetVirtualRootFolderAsync();
        options ??= new ResourceQueryOptions(false, true);
        var resource =
            await Repository.GetAsync(id, root.Id, options, CancellationToken);
        if (resource == null)
        {
            throw new EntityNotFoundBusinessException(Localizer["Path"], id);
        }

        return resource;
    }

    public virtual async Task<Resource> FindVirtualPathAsync(string virtualPath, ResourceQueryOptions? options = null)
    {
        var root = await GetVirtualRootFolderAsync();
        options ??= new ResourceQueryOptions(false, true);
        var resource = await Repository.FindAsync(
            virtualPath, root.Id, options, CancellationToken);
        if (resource == null)
        {
            throw new EntityNotFoundBusinessException(Localizer["Path"], virtualPath);
        }

        return resource;
    }

    public virtual async Task<Tuple<long, List<Resource>>> GetVirtualPathsAsync(
        ResourceSearchAndPagedAndSortedParams search)
    {
        var root = await GetVirtualRootFolderAsync();
        search.Sorting ??= $"{nameof(Resource.Name)}";
        var predicate = await Repository.BuildQueryExpressionAsync(root.Id, search);
        var count = await Repository.GetCountAsync(predicate, CancellationToken);
        var resources =
            await Repository.GetListAsync(predicate, search, new ResourceQueryOptions(false, true), CancellationToken);
        return Tuple.Create(count, resources);
    }

    public virtual async Task<Resource> CreateVirtualPathAsync(string name, Guid folderId)
    {
        await ValidateIsPublicFolderAsync(folderId);
        var root = await GetVirtualRootFolderAsync();
        var entity = new Resource(GuidGenerator.Create(), name, ResourceType.VirtualPath, false,
            CurrentTenant.Id);
        entity.SetFolderId(folderId);
        entity.MoveTo(root.Id);
        await CreateAsync(entity);
        Logger.LogInformation("Created virtual path {id} with name {name} and folder {currentTenantId}.{folderId}",
            entity.Id, entity.Name, CurrentTenant.Id.HasValue ? CurrentTenant.Id.ToString() : "HOST", folderId);
        return entity;
    }

    public virtual async Task<Resource> UpdateVirtualPathAsync(Guid id, string name, Guid folderId)
    {
        await ValidateIsPublicFolderAsync(folderId);
        var entity = await GetVirtualPathAsync(id, new ResourceQueryOptions(false));
        entity.Rename(name);
        entity.SetFolderId(folderId);
        await UpdateAsync(entity);
        Logger.LogInformation("Updated virtual path {id} with name {name} and folder {currentTenantId}.{folderId}",
            entity.Id, entity.Name, CurrentTenant.Id.HasValue ? CurrentTenant.Id.ToString() : "HOST", folderId);
        return entity;
    }


    public virtual async Task DeleteVirtualPathAsync(Guid id)
    {
        var entity = await GetVirtualPathAsync(id, new ResourceQueryOptions(false));
        ValidateIsStaticFolder(entity);
        await DeleteAsync(entity);
    }

    public virtual async Task<List<ResourcePermission>> GetVirtualPathPermissionAsync(Guid id)
    {
        var entity = await GetVirtualPathAsync(id);
        return await GetPermissionsAsync(entity);
    }

    public virtual async Task UpdateVirtualPathPermissionAsync(Guid id, List<ResourcePermission> permissions)
    {
        var entity = await GetVirtualPathAsync(id);
        await SetPermissionsAsync(entity, permissions);
    }
}