using Generic.Abp.FileManagement.Settings.Result;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Generic.Abp.FileManagement.Resources;

/// <summary>
/// Root folder methods  for <see cref="ResourceManager"/>.
/// </summary>
public partial class ResourceManager
{
    public virtual async Task<ResourceCacheItem> GetPublicRootFolderAsync()
    {
        return await GetRootFolderAsync(ResourceType.PublicRootFolder);
    }

    public virtual async Task<ResourceCacheItem> GetSharedRootFolderAsync()
    {
        return await GetRootFolderAsync(ResourceType.SharedRootFolder);
    }

    public virtual async Task<ResourceCacheItem> GetUsersRootFolderAsync()
    {
        return await GetRootFolderAsync(ResourceType.UsersRootFolder);
    }

    public virtual async Task<ResourceCacheItem> GetVirtualRootFolderAsync()
    {
        return await GetRootFolderAsync(ResourceType.VirtualRootFolder);
    }

    public virtual async Task<ResourceCacheItem> GetParticipantIsolationRootFolderAsync()
    {
        return await GetRootFolderAsync(ResourceType.ParticipantIsolationRootFolder);
    }

    public virtual async Task<ResourceCacheItem> GetRootFolderAsync(ResourceType type)
    {
        var tenantId = CurrentTenant.Id;
        var key = $"{type.ToString()}_{tenantId.ToString() ?? "host"}";
        var resource = await Cache.GetAsync(key);
        if (resource != null)
        {
            return resource;
        }

        var entity = type switch
        {
            ResourceType.SharedRootFolder => await CreateSharedRootFolderAsync(CurrentTenant.Id),
            ResourceType.UsersRootFolder => await CreateUsersRootFolderAsync(CurrentTenant.Id),
            ResourceType.VirtualRootFolder => await CreateVirtualPathRootFolderAsync(CurrentTenant.Id),
            ResourceType.ParticipantIsolationRootFolder => await CreateParticipantIsolationRootFolderAsync(CurrentTenant
                .Id),
            _ => await CreatePublicRootFolderAsync()
        };

        resource = new ResourceCacheItem(entity.Id, entity.Code, entity.Name);
        await Cache.SetAsync(key, resource);

        return resource;
    }

    public virtual async Task<Resource> GetUserRootFolderAsync(Guid userId, Guid? tenantId = null)
    {
        var entity = await CreateRootFolderAsync(ResourceType.UserRootFolder,
            await SettingManager.GetUserFolderSettingAsync(), tenantId, userId);
        return entity;
    }

    public virtual async Task<Resource> CreatePublicRootFolderAsync(Guid? tenantId = null)
    {
        return await CreateRootFolderAsync(ResourceType.PublicRootFolder,
            await SettingManager.GetPublicFolderSettingAsync(),
            tenantId);
    }

    public virtual async Task<Resource> CreateSharedRootFolderAsync(Guid? tenantId = null)
    {
        return await CreateRootFolderAsync(ResourceType.SharedRootFolder,
            await SettingManager.GetSharedFolderSettingAsync(),
            tenantId);
    }

    public virtual async Task<Resource> CreateUsersRootFolderAsync(Guid? tenantId = null)
    {
        return await CreateRootFolderAsync(ResourceType.UsersRootFolder,
            await SettingManager.GetUserFolderSettingAsync(),
            tenantId);
    }

    public virtual async Task<Resource> CreateVirtualPathRootFolderAsync(Guid? tenantId = null)
    {
        return await CreateRootFolderAsync(ResourceType.VirtualRootFolder,
            await SettingManager.GetVirtualPathSettingAsync(),
            tenantId);
    }

    public virtual async Task<Resource> CreateParticipantIsolationRootFolderAsync(Guid? tenantId = null)
    {
        return await CreateRootFolderAsync(ResourceType.ParticipantIsolationRootFolder,
            await SettingManager.GetParticipantIsolationFolderSettingAsync(),
            tenantId);
    }


    protected virtual async Task<Resource> CreateRootFolderAsync(
        ResourceType rooType,
        FolderSetting settings,
        Guid? tenantId = null,
        Guid? userId = null,
        bool isEnabled = true)
    {
        var name = await GetRootFolderNameAsync(rooType, tenantId, userId);
        ResourceCacheItem? usersRootFolder = null;
        if (userId.HasValue)
        {
            usersRootFolder = await GetUsersRootFolderAsync();
        }

        var existingResource = userId.HasValue
            ? await FindAsync(m => m.ParentId == usersRootFolder!.Id && m.OwnerId == userId.Value, null,
                new ResourceQueryOptions(false, includeConfiguration: true))
            : await FindAsync(m => m.Type == rooType, null,
                new ResourceQueryOptions(false, includeConfiguration: true));
        if (existingResource == null)
        {
            // Create the folder only if it does not exist
            Logger.LogInformation("Creating {name} root folder.", name);
            var entity = new Resource(GuidGenerator.Create(), name, rooType, true, tenantId);
            if (userId.HasValue)
            {
                entity.MoveTo(usersRootFolder!.Id);
                entity.SetOwner(userId.Value);
                entity.SetIsEnabled(isEnabled);
            }

            await CreateOrUpdateConfigurationAsync(entity, settings.AllowFileTypes, settings.StorageQuota,
                settings.MaxFileSize);
            await CreateAsync(entity);
            return entity;
        }

        Logger.LogInformation("{name} root folder already exists, skipping creation.", name);

        return existingResource;
    }

    protected virtual Task<string> GetRootFolderNameAsync(ResourceType type, Guid? tenantId = null, Guid? userId = null)
    {
        var name = type + (tenantId.ToString() ?? "host") + "_" + (userId.ToString() ?? "");
        return Task.FromResult(name);
    }

    public virtual Task<string> GetUserRootFolderNameAsync(Guid userId)
    {
        return Task.FromResult(ResourceConsts.UserFolder.Name + "_" + userId);
    }
}