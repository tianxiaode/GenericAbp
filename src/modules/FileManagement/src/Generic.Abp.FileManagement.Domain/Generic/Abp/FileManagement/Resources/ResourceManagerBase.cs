using System;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Extensions;
using Generic.Abp.Extensions.Trees;
using Generic.Abp.FileManagement.Localization;
using Generic.Abp.FileManagement.Settings;
using Microsoft.Extensions.Localization;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.SettingManagement;
using Volo.Abp.Threading;

namespace Generic.Abp.FileManagement.Resources;

public class ResourceManagerBase(
    IResourceRepository repository,
    ITreeCodeGenerator<Resource> treeCodeGenerator,
    IStringLocalizer<FileManagementResource> localizer,
    IDistributedCache<ResourceCacheItem, string> cache,
    ResourcePermissionManager resourcePermissionManager,
    ISettingManager settingManager,
    IDistributedEventBus distributedEventBus,
    ICancellationTokenProvider cancellationTokenProvider)
    : TreeManager<Resource, IResourceRepository>(repository, treeCodeGenerator, cancellationTokenProvider)
{
    protected IStringLocalizer<FileManagementResource> Localizer { get; } = localizer;
    protected IDistributedCache<ResourceCacheItem, string> Cache { get; } = cache;
    protected ResourcePermissionManager PermissionManager { get; } = resourcePermissionManager;
    protected ISettingManager SettingManager { get; } = settingManager;
    protected IDistributedEventBus DistributedEventBus { get; } = distributedEventBus;


    #region Folder methods

    public virtual Task<bool> IsRooFolderAsync(Resource entity)
    {
        return Task.FromResult(entity.ParentId == null);
    }

    public virtual async Task<bool> IsPublicFolderAsync(Resource entity)
    {
        var publicRoot = await GetPublicRootFolderAsync();
        return entity.Code.StartsWith(publicRoot.Code);
    }

    public virtual async Task<bool> IsUsersFolderAsync(Resource entity)
    {
        var privateRoot = await GetUsersRootFolderAsync();
        return entity.Code.StartsWith(privateRoot.Code);
    }

    public virtual async Task<bool> IsSharedFolderAsync(Resource entity)
    {
        var sharedRoot = await GetSharedRootFolderAsync();
        return entity.Code.StartsWith(sharedRoot.Code);
    }

    public virtual async Task<bool> IsVirtualFolderAsync(Resource entity)
    {
        var virtualRoot = await GetVirtualRootFolderAsync();
        return entity.Code.StartsWith(virtualRoot.Code);
    }

    public virtual async Task<ResourceCacheItem> GetPublicRootFolderAsync()
    {
        return await GetRootFolderAsync(ResourceConsts.PublicRootFolderName);
    }

    public virtual async Task<ResourceCacheItem> GetSharedRootFolderAsync()
    {
        return await GetRootFolderAsync(ResourceConsts.SharedRootFolderName);
    }

    public virtual async Task<ResourceCacheItem> GetUsersRootFolderAsync()
    {
        return await GetRootFolderAsync(ResourceConsts.UsersRootFolderName);
    }

    public virtual async Task<ResourceCacheItem> GetVirtualRootFolderAsync()
    {
        return await GetRootFolderAsync(ResourceConsts.VirtualRootFolderName);
    }

    public virtual async Task<ResourceCacheItem> GetRootFolderAsync(string folderName)
    {
        var tenantId = CurrentTenant.Id;
        var key = $"root_{folderName}_{tenantId.ToString() ?? "host"}";
        var resource = await Cache.GetAsync(key);
        if (resource != null)
        {
            return resource;
        }

        var entity = await Repository.FirstOrDefaultAsync(m => m.Name == folderName);
        if (entity == null)
        {
            entity = new Resource(GuidGenerator.Create(), folderName, ResourceType.Folder, true, tenantId);
            await CreateAsync(entity);
        }

        resource = new ResourceCacheItem(entity.Id, entity.Code, entity.Name);
        await Cache.SetAsync(key, resource);

        return resource;
    }

    public virtual async Task<Resource> GetUserRootFolderAsync(Guid userId)
    {
        var userRootFolderName = await GetUserRootFolderNameAsync(userId);
        var entity = await Repository.FirstOrDefaultAsync(m => m.Name == userRootFolderName);
        if (entity != null)
        {
            return entity;
        }

        var parent = await GetUsersRootFolderAsync();
        entity = new Resource(GuidGenerator.Create(), userRootFolderName, ResourceType.Folder, true, CurrentTenant.Id);
        var quotaStr =
            await SettingManager.GetOrNullForCurrentTenantAsync(FileManagementSettings.UserFolder..DefaultQuota) ??
            ResourceConsts.UserFolder.DefaultQuota;
        var maxFileSizeStr =
            await SettingManager.GetOrNullForCurrentTenantAsync(FileManagementSettings.Users.DefaultFileMaxSize) ??
            ResourceConsts.UserFolder.DefaultFileMaxSize;
        var allowedFileTypesStr =
            await SettingManager.GetOrNullForCurrentTenantAsync(FileManagementSettings.Users.DefaultFileTypes) ??
            ResourceConsts.UserFolder.DefaultFileTypes;
        entity.SetQuota(quotaStr.ParseToBytes());
        entity.SetMaxFileSize(maxFileSizeStr.ParseToBytes());
        entity.SetAllowedFileTypes(allowedFileTypesStr);
        await CreateAsync(entity);

        return entity;
    }

    public virtual Task<string> GetUserRootFolderNameAsync(Guid userId)
    {
        return Task.FromResult(ResourceConsts.UsersRootFolderName + "_" + userId);
    }

    #endregion
}