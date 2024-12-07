using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.FileManagement.Exceptions;
using Generic.Abp.FileManagement.Settings;
using Medallion.Threading;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;

namespace Generic.Abp.FileManagement.Resources;

/// <summary>
/// 管理与文件夹相关的资源
/// </summary>
public partial class ResourceManager
{
    public virtual async Task<Resource> FindFolderByNameAsync(Guid parentId, string name)
    {
        var entity = await FindAsync(m => m.ParentId == parentId && m.Name.ToLower() == name.ToLower());
        if (entity == null)
        {
            throw new EntityNotFoundBusinessException(L["Folder"], name);
        }

        return entity;
    }


    private async Task UpdateUsedStorageAsync(Resource entity, long addSize)
    {
        // 获取分布式锁
        var lockKey = await GetUpdateUsedStorageLockNameAsync(entity.Id);
        await using var distributedLock =
            await DistributedLockProvider.TryAcquireLockAsync(lockKey, TimeSpan.FromSeconds(10));

        if (distributedLock == null)
        {
            throw new AbpException($"Failed to acquire lock for {lockKey} used storage adjustment.");
        }

        try
        {
            // 更新实体的存储使用量
            var currentUsedStorage = entity.GetUsedStorage();
            entity.SetUsedStorage(currentUsedStorage + addSize);

            // 使用事务确保更新操作的一致性
            await Repository.UpdateAsync(entity);
        }
        catch (Exception ex)
        {
            // 记录详细的异常信息到日志系统
            Logger.LogError(ex, "Error acquiring lock or updating used storage for resource {ResourceId}", entity.Id);
            throw; // 重新抛出异常以保持方法的透明性
        }
    }

    public virtual async Task IncreaseUsedStorageAsync(Resource entity, long addSize)
    {
        if (addSize <= 0)
        {
            return;
        }

        var currentUsedStorage = entity.GetUsedStorage();
        var quota = entity.GetStorageQuota();

        // 检查是否会超出配额
        if (currentUsedStorage + addSize > quota)
        {
            throw new InsufficientStorageSpaceBusinessException(addSize, currentUsedStorage, quota);
        }

        await UpdateUsedStorageAsync(entity, addSize);
    }

    public virtual async Task DecreaseUsedStorageAsync(Resource entity, long subtractSize)
    {
        if (subtractSize < 0)
        {
            throw new ArgumentException("Subtract size cannot be negative.", nameof(subtractSize));
        }

        await UpdateUsedStorageAsync(entity, -subtractSize);
    }

    protected virtual Task<string> GetUpdateUsedStorageLockNameAsync(Guid id)
    {
        return Task.FromResult($"Lock:FileManagement:Folder:UpdateUsedStorage:{id}");
    }

    public virtual async Task<Resource> CreateFolderAsync(string name, Guid parentId, string? allowFileTypes,
        string? quota, string? maxFileSize, Guid? tenantId = null)
    {
        await ValidateIsPublicOrIsSharedFolderAsync(parentId);

        var entity = new Resource(GuidGenerator.Create(), name, ResourceType.Folder, false, tenantId);
        entity.MoveTo(parentId);
        await CreateAsync(entity);
        return entity;
    }

    public virtual async Task<Resource> UpdateFolderAsync(Guid id, string name, string? allowFileTypes, string? quota,
        string? maxFileSize)
    {
        var entity = await Repository.GetAsync(id);
        if (!entity.ParentId.HasValue)
        {
            if (!await IsPublicFolderAsync(entity) || !await IsSharedFolderAsync(entity))
            {
                throw new EntityNotFoundBusinessException(L["Folder"], name);
            }
        }
        else
        {
            await ValidateIsPublicOrIsSharedFolderAsync(entity.ParentId.Value);
        }


        entity.Rename(name);

        await UpdateAsync(entity);
        return entity;
    }

    public virtual async Task MoveFolderAsync(Guid id, Guid parentId)
    {
        if (parentId == id)
        {
            throw new CanNotMoveOrCopyToItselfBusinessException();
        }

        var entity = await Repository.GetAsync(id);

        if (!await IsPublicFolderAsync(entity))
        {
            throw new OnlyMovePublicFolderBusinessException();
        }

        ValidateIsStaticFolder(entity);

        var parent = await Repository.GetAsync(parentId);
        if (parent.Type != ResourceType.Folder)
        {
            throw new EntityNotFoundBusinessException(L["Folder"], parentId);
        }

        if (parent.Code.StartsWith(entity.Code))
        {
            throw new CanNotMoveToChildBusinessException();
        }

        if (!await IsPublicFolderAsync(parent))
        {
            throw new OnlyMovePublicFolderBusinessException();
        }

        await MoveAsync(entity, parentId);
    }

    public virtual async Task CopyFolderAsync(Guid id, Guid parentId)
    {
        if (parentId == id)
        {
            throw new CanNotMoveOrCopyToItselfBusinessException();
        }

        var entity = await Repository.GetAsync(id);
        if (entity.Type != ResourceType.Folder)
        {
            throw new EntityNotFoundBusinessException(L["Folder"], parentId);
        }

        var maxNodeCount = await SettingManager.GetSettingAsync<int>(FileManagementSettings.FolderCopyMaxNodeCount);
        var count = await Repository.GetAllChildrenCountByCodeAsync(entity.Code, CancellationToken);
        if (count >= maxNodeCount)
        {
            throw new OnlyMoveMaxFilesAndFoldersInOnTimeBusinessException(maxNodeCount, count);
        }

        var parent = await Repository.GetAsync(parentId);
        if (parent.Type != ResourceType.Folder)
        {
            throw new EntityNotFoundBusinessException(L["Folder"], parentId);
        }

        await CopyAsync(entity, parentId);
    }

    public virtual async Task DeleteFolderAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        ValidateIsStaticFolder(entity);
        await DeleteAsync(entity);
    }

    public virtual async Task<List<Resource>> GetSearchFoldersAsync(string filter)
    {
        return await Repository.GetListAsync(m => m.Name.Contains(filter) && m.Type == ResourceType.Folder, true,
            CancellationToken);
    }

    public virtual async Task<List<Resource>> GetChildrenFoldersAsync(Guid parentId)
    {
        return await Repository.GetListAsync(m => m.ParentId == parentId && m.Type == ResourceType.Folder, true,
            CancellationToken);
    }
}