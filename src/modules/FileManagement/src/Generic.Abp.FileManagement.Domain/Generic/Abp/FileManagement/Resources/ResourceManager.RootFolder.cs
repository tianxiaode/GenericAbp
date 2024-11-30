using System;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Extensions;
using Generic.Abp.FileManagement.Settings;
using Generic.Abp.FileManagement.Settings.Result;
using Generic.Abp.VirtualPaths;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Repositories;

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
            SettingManager.GetUserFolderSettingAsync, tenantId, userId);
        return entity;
    }

    public virtual async Task<Resource> CreatePublicRootFolderAsync(Guid? tenantId = null)
    {
        return await CreateRootFolderAsync(ResourceType.PublicRootFolder, SettingManager.GetPublicFolderSettingAsync,
            tenantId);
    }

    public virtual async Task<Resource> CreateSharedRootFolderAsync(Guid? tenantId = null)
    {
        return await CreateRootFolderAsync(ResourceType.SharedRootFolder, SettingManager.GetSharedFolderSettingAsync,
            tenantId);
    }

    public virtual async Task<Resource> CreateUsersRootFolderAsync(Guid? tenantId = null)
    {
        return await CreateRootFolderAsync(ResourceType.UsersRootFolder, SettingManager.GetUserFolderSettingAsync,
            tenantId);
    }

    public virtual async Task<Resource> CreateVirtualPathRootFolderAsync(Guid? tenantId = null)
    {
        return await CreateRootFolderAsync(ResourceType.VirtualRootFolder, SettingManager.GetVirtualPathSettingAsync,
            tenantId);
    }

    public virtual async Task<Resource> CreateParticipantIsolationRootFolderAsync(Guid? tenantId = null)
    {
        return await CreateRootFolderAsync(ResourceType.ParticipantIsolationRootFolder,
            SettingManager.GetParticipantIsolationFolderSettingAsync,
            tenantId);
    }


    protected virtual async Task<Resource> CreateRootFolderAsync(
        ResourceType rooType,
        Func<Task<FolderSetting>> getFolderSetting,
        Guid? tenantId = null,
        Guid? userId = null)
    {
        var name = await GetRootFolderNameAsync(rooType, tenantId, userId);
        ResourceCacheItem? usersRootFolder = null;
        if (userId.HasValue)
        {
            usersRootFolder = await GetUsersRootFolderAsync();
        }

        var existingResource = userId.HasValue
            ? await Repository.FindAsync(m => m.ParentId == usersRootFolder!.Id && m.OwnerId == userId.Value)
            : await Repository.FindAsync(m => m.Type == rooType);
        if (existingResource == null)
        {
            // Create the folder only if it does not exist
            Logger.LogInformation("Creating {name} root folder.", name);
            var resource = new Resource(GuidGenerator.Create(), name, rooType, true, tenantId);
            var settings = await getFolderSetting();
            if (userId.HasValue)
            {
                resource.MoveTo(usersRootFolder!.Id);
                resource.SetOwner(userId.Value);
            }

            var configuration = new ResourceConfiguration(GuidGenerator.Create(), settings.FileTypes,
                settings.Quota.ParseToBytes(), 0, settings.FileMaxSize.ParseToBytes(), tenantId);
            await ResourceConfigurationRepository.InsertAsync(configuration);
            resource.SetConfiguration(configuration.Id);
            await Repository.InsertAsync(resource);
            return resource;
        }
        else
        {
            Logger.LogInformation("{name} root folder already exists, skipping creation.", name);
        }

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