using System;
using System.Threading.Tasks;
using Generic.Abp.VirtualPaths;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.FileManagement.Resources;

/// <summary>
/// Root folder methods  for <see cref="VirtualPaths.ResourceManager"/>.
/// </summary>
public partial class ResourceManager
{
    public virtual async Task<ResourceCacheItem> GetPublicRootFolderAsync()
    {
        return await GetRootFolderAsync(ResourceConsts.PublicFolder.Name);
    }

    public virtual async Task<ResourceCacheItem> GetSharedRootFolderAsync()
    {
        return await GetRootFolderAsync(ResourceConsts.SharedFolder.Name);
    }

    public virtual async Task<ResourceCacheItem> GetUsersRootFolderAsync()
    {
        return await GetRootFolderAsync(ResourceConsts.UserFolder.Name);
    }

    public virtual async Task<ResourceCacheItem> GetVirtualRootFolderAsync()
    {
        return await GetRootFolderAsync(ResourceConsts.VirtualPath.Name);
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
            var settings = folderName switch
            {
                ResourceConsts.PublicFolder.Name => await SettingManager.GetPublicFolderSettingAsync(),
                ResourceConsts.SharedFolder.Name => await SettingManager.GetSharedFolderSettingAsync(),
                ResourceConsts.UserFolder.Name => await SettingManager.GetUserFolderSettingAsync(),
                _ => await SettingManager.GetVirtualPathSettingAsync()
            };

            entity.SetQuota(settings.Quota);
            entity.SetMaxFileSize(settings.FileMaxSize);
            entity.SetAllowedFileTypes(settings.FileTypes);

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
        var settings = await SettingManager.GetUserFolderSettingAsync();
        entity = new Resource(GuidGenerator.Create(), userRootFolderName, ResourceType.Folder, true, CurrentTenant.Id);
        entity.MoveTo(parent.Id);
        entity.SetQuota(settings.Quota);
        entity.SetMaxFileSize(settings.FileMaxSize);
        entity.SetAllowedFileTypes(settings.FileTypes);
        await CreateAsync(entity);

        return entity;
    }

    public virtual Task<string> GetUserRootFolderNameAsync(Guid userId)
    {
        return Task.FromResult(ResourceConsts.UserFolder.Name + "_" + userId);
    }
}