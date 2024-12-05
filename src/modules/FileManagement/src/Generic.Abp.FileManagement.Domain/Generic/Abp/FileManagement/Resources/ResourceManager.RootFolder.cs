using Microsoft.Extensions.Logging;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.Settings;
using Volo.Abp;

namespace Generic.Abp.FileManagement.Resources;

/// <summary>
/// Root folder methods  for <see cref="ResourceManager"/>.
/// </summary>
public partial class ResourceManager
{
    /// <summary>
    /// 获取公共根文件夹。
    /// </summary>
    /// <param name="tenantId"> 租户Id。 </param>
    /// <returns>公共根文件夹的缓存项。</returns>
    public virtual Task<ResourceCacheItem> GetPublicRootFolderAsync(Guid? tenantId = null) =>
        GetRootFolderCacheItemAsync(ResourceType.PublicRootFolder, tenantId);

    /// <summary>
    /// 获取共享根文件夹。
    /// </summary>
    /// <param name="tenantId"> 租户Id。 </param>
    /// <returns>共享根文件夹的缓存项。</returns>
    public virtual Task<ResourceCacheItem> GetSharedRootFolderAsync(Guid? tenantId = null) =>
        GetRootFolderCacheItemAsync(ResourceType.SharedRootFolder, tenantId);

    /// <summary>
    /// 获取用户根文件夹。
    /// </summary>
    /// <param name="tenantId"> 租户Id。 </param>
    /// <returns> 用户根文件夹的缓存项。</returns>
    public virtual Task<ResourceCacheItem> GetUsersRootFolderAsync(Guid? tenantId = null) =>
        GetRootFolderCacheItemAsync(ResourceType.UsersRootFolder, tenantId);

    /// <summary>
    /// 获取参与者隔离根文件夹。
    /// </summary>
    /// <param name="tenantId"> 租户Id。 </param>
    /// <returns> 参与者隔离根文件夹的缓存项。</returns>
    public virtual Task<ResourceCacheItem> GetParticipantIsolationsRootFolderAsync(Guid? tenantId = null) =>
        GetRootFolderCacheItemAsync(ResourceType.ParticipantIsolationRootFolder, tenantId);

    /// <summary>
    /// 获取根文件夹缓存项。
    /// </summary>
    /// <param name="type"> 根文件夹类型。 </param>
    /// <param name="tenantId"> 租户Id。 </param>
    /// <returns> 根文件夹的缓存项。</returns>
    /// <exception cref="AbpException"></exception>
    public virtual async Task<ResourceCacheItem> GetRootFolderCacheItemAsync(ResourceType type, Guid? tenantId = null)
    {
        var key = await GetRootFolderNameAsync(type, tenantId);
        var resource = await Cache
            .GetOrAddAsync(key, async () => await CreateResourceCacheItem(type, tenantId), token: CancellationToken);
        if (resource == null)
        {
            throw new AbpException($"Root folder for {type} not found.");
        }

        return resource;
    }

    /// <summary>
    /// 获取用户根文件夹。
    /// </summary>
    /// <param name="userId">  用户Id。 </param>
    /// <param name="tenantId"> 租户Id。 </param>
    /// <param name="isAccessible"> 是否可访问。 </param>
    /// <returns> 用户根文件夹。</returns>
    public virtual async Task<Resource> GetUserRootFolderAsync(Guid userId, Guid? tenantId = null,
        bool isAccessible = true)
    {
        var root = await GetUsersRootFolderAsync();
        var existing = await GetRootFolderAsync(m => m.ParentId == root.Id && m.OwnerId == userId);
        if (existing != null)
        {
            return existing;
        }

        var entity = new Resource(GuidGenerator.Create(),
            await GetRootFolderNameAsync(ResourceType.UserRootFolder, tenantId, userId),
            ResourceType.Folder, true, tenantId);
        entity.MoveTo(root.Id);
        entity.SetOwner(userId);
        entity.SetIsAccessible(isAccessible);
        await ConfigureAndCreateRootFolderAsync(entity, FileManagementSettings.UserFolderPrefix);
        return entity;
    }

    protected async Task<ResourceCacheItem> CreateResourceCacheItem(ResourceType type, Guid? tenantId = null)
    {
        var entity = await GetRootFolderAsync(m => m.Type == type) ?? await CreateRootFolderAsync(type, tenantId);
        return new ResourceCacheItem(entity.Id, entity.Code, entity.Name);
    }

    protected virtual async Task<Resource?> GetRootFolderAsync(Expression<Func<Resource, bool>> predicate) =>
        await FindAsync(predicate, new ResourceIncludeOptions(false));

    protected virtual async Task<Resource> CreateRootFolderAsync(ResourceType type, Guid? tenantId = null)
    {
        var name = await GetRootFolderNameAsync(type, tenantId);
        var settingPrefix = GetSettingPrefixForType(type);
        var entity = new Resource(GuidGenerator.Create(), name, type, true, tenantId);
        await ConfigureAndCreateRootFolderAsync(entity, settingPrefix);
        return entity;
    }

    protected virtual string GetSettingPrefixForType(ResourceType type) =>
        type switch
        {
            ResourceType.PublicRootFolder => FileManagementSettings.PublicFolderPrefix,
            ResourceType.SharedRootFolder => FileManagementSettings.SharedFolderPrefix,
            ResourceType.UsersRootFolder => FileManagementSettings.UserFolderPrefix,
            ResourceType.ParticipantIsolationRootFolder => FileManagementSettings.ParticipantIsolationFolderPrefix,
            _ => throw new AbpException("Invalid root folder type.")
        };

    protected virtual async Task ConfigureAndCreateRootFolderAsync(Resource entity, string settingPrefix)
    {
        try
        {
            var settings = await SettingManager.GetFolderSettingAsync(settingPrefix);
            entity.SetHasConfiguration(true);
            entity.SetAllowedFileTypes(settings.AllowFileTypes);
            entity.SetMaxFileSize(settings.MaxFileSize);
            entity.SetStorageQuota(settings.StorageQuota);
            entity.SetAllowedFileCount(settings.AllowFileCount);
            entity.SetUsedStorage(0);
            await CreateAsync(entity);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "An error occurred while creating {name} root folder for tenant {tenantId}.",
                entity.Name,
                entity.TenantId);
            throw new AbpException("An error occurred while creating root folder.", e);
        }
    }

    protected virtual Task<string> GetRootFolderNameAsync(ResourceType type, Guid? tenantId = null, Guid? userId = null)
    {
        var tenantIdStr = tenantId.HasValue ? tenantId.Value.ToString() : "host";
        var userIdStr = userId.HasValue ? "_" + userId.Value : "";
        var name = $"{type.ToString()}_{tenantIdStr}{userIdStr}";
        return Task.FromResult(name);
    }
}