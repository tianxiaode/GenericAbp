using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.Extensions.Extensions;
using Generic.Abp.Extensions.Trees;
using Generic.Abp.FileManagement.Events;
using Generic.Abp.FileManagement.Localization;
using Generic.Abp.FileManagement.Settings;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.SettingManagement;
using Volo.Abp.Threading;

namespace Generic.Abp.FileManagement.Folders;

public class FolderManager(
    IFolderRepository repository,
    ITreeCodeGenerator<Folder> treeCodeGenerator,
    ICancellationTokenProvider cancellationTokenProvider,
    IStringLocalizer<FileManagementResource> localizer,
    IDistributedCache<FolderCacheItem, string> cache,
    FolderPermissionManager folderPermissionManager,
    ISettingManager settingManager,
    IDistributedEventBus distributedEventBus
)
    : TreeManager<Folder, IFolderRepository>(repository, treeCodeGenerator,
        cancellationTokenProvider)
{
    protected IStringLocalizer<FileManagementResource> Localizer { get; } = localizer;
    protected IDistributedCache<FolderCacheItem, string> Cache { get; } = cache;
    protected FolderPermissionManager FolderPermissionManager { get; } = folderPermissionManager;
    protected ISettingManager SettingManager { get; } = settingManager;
    protected IDistributedEventBus DistributedEventBus { get; } = distributedEventBus;

    public override async Task CreateAsync(Folder entity, bool autoSave = true)
    {
        await base.CreateAsync(entity, autoSave);
        if (entity is { IsInheritPermissions: true, ParentId: not null })
        {
            var permissions =
                await FolderPermissionManager.GetListAsync(entity.ParentId.Value, CancellationToken);
            await SetPermissionsAsync(entity, permissions);
        }
    }

    public override async Task DeleteAsync(Folder entity, bool autoSave = true)
    {
        await SetPermissionsAsync(entity, []);
        await base.DeleteAsync(entity, autoSave);
    }

    public override async Task ValidateAsync(Folder entity)
    {
        if (await Repository.AnyAsync(m =>
                m.ParentId == entity.ParentId && m.Id != entity.Id &&
                m.Name.ToLower() == entity.Name.ToLowerInvariant()))
        {
            throw new DuplicateWarningBusinessException(Localizer[nameof(Folder)], entity.Name);
        }
    }

    #region files

    public virtual async Task<bool> FileExistAsync(Guid folderId, Guid fileId)
    {
        return await Repository.FilesExistAsync(folderId, fileId, CancellationToken);
    }

    #endregion

    #region permissions

    public virtual async Task<List<FolderPermission>> GetPermissionsAsync(Folder folder)
    {
        return await FolderPermissionManager.GetListAsync(folder.Id, CancellationToken);
    }

    public virtual async Task SetPermissionsAsync(Folder folder, List<FolderPermission> permissions)
    {
        var currentPermissions = await FolderPermissionManager.GetListAsync(folder.Id, CancellationToken);

        var currentPermissionIds = new HashSet<Guid>(currentPermissions.Select(m => m.Id));
        var newPermissionIds = new HashSet<Guid>(permissions.Select(m => m.Id));

        // 找出需要删除的权限
        var removePermissions = currentPermissions.Where(m => !newPermissionIds.Contains(m.Id)).ToList();
        await FolderPermissionManager.DeleteManyAsync(removePermissions, CancellationToken);

        // 找出需要新增的权限
        var insertPermissions = permissions.Where(m => !currentPermissionIds.Contains(m.Id)).ToList();
        await FolderPermissionManager.InsertManyAsync(insertPermissions, CancellationToken);

        // 找出需要更新的权限
        var updatePermissions = permissions.Where(m => currentPermissionIds.Contains(m.Id)).ToList();
        await FolderPermissionManager.UpdateManyAsync(updatePermissions, CancellationToken);
    }


    public virtual async Task<bool> CadReadAsync(Folder folder, Guid userId)
    {
        if (await FolderPermissionManager.AllowAuthenticatedUserReadAsync(folder.Id, CancellationToken))
        {
            return true;
        }

        return await FolderPermissionManager.AllowUserOrRolesReadAsync(folder.Id, userId, CancellationToken);
    }

    public virtual async Task<bool> CadWriteAsync(Folder folder, Guid userId)
    {
        if (await FolderPermissionManager.AllowAuthenticatedUserWriteAsync(folder.Id, CancellationToken))
        {
            return true;
        }

        return await FolderPermissionManager.AllowUserOrRolesWriteAsync(folder.Id, userId, CancellationToken);
    }

    public virtual async Task<bool> CadDeleteAsync(Folder folder, Guid userId)
    {
        if (await FolderPermissionManager.AllowAuthenticatedUserDeleteAsync(folder.Id, CancellationToken))
        {
            return true;
        }

        return await FolderPermissionManager.AllowUserOrRolesDeleteAsync(folder.Id, userId, CancellationToken);
    }

    #endregion


    public virtual async Task<bool> IsPublicFolderAsync(Folder folder)
    {
        var publicRoot = await GetPublicRootFolderAsync();
        return folder.Code.StartsWith(publicRoot.Code);
    }

    public virtual async Task<bool> IsPrivateFolderAsync(Folder folder)
    {
        var privateRoot = await GetUsersRootFolderAsync();
        return folder.Code.StartsWith(privateRoot.Code);
    }

    public virtual async Task<bool> IsSharedFolderAsync(Folder folder)
    {
        var sharedRoot = await GetSharedRootFolderAsync();
        return folder.Code.StartsWith(sharedRoot.Code);
    }

    public virtual async Task<FolderCacheItem> GetPublicRootFolderAsync()
    {
        return await GetRootFolderAsync(FolderConsts.PublicRootFolderName);
    }

    public virtual async Task<FolderCacheItem> GetSharedRootFolderAsync()
    {
        return await GetRootFolderAsync(FolderConsts.SharedRootFolderName);
    }

    public virtual async Task<FolderCacheItem> GetUsersRootFolderAsync()
    {
        return await GetRootFolderAsync(FolderConsts.UsersRootFolderName);
    }

    public virtual async Task<FolderCacheItem> GetRootFolderAsync(string folderName)
    {
        var tenantId = CurrentTenant.Id?.ToString() ?? "host";
        var key = $"root_{folderName}_{tenantId}";
        var folder = await Cache.GetAsync(key);
        if (folder != null)
        {
            return folder;
        }

        var entity = await Repository.FirstOrDefaultAsync(m => m.Name == folderName);
        if (entity == null)
        {
            throw new UserFriendlyException($"Root folder not found: {folderName}");
        }

        folder = new FolderCacheItem(entity.Id, entity.Code, entity.Name);
        await Cache.SetAsync(key, folder);

        return folder;
    }

    public virtual async Task<Folder> GetUserRootFolderAsync(Guid userId)
    {
        var userRootFolderName = await GetUserRootFolderNameAsync(userId);
        var entity = await Repository.FirstOrDefaultAsync(m => m.Name == userRootFolderName);
        if (entity != null)
        {
            return entity;
        }

        var parent = await GetUsersRootFolderAsync();
        entity = new Folder(GuidGenerator.Create(), parent.Id, userRootFolderName, true, CurrentTenant.Id);
        var quotaStr = await SettingManager.GetOrNullForCurrentTenantAsync(FileManagementSettings.Users.DefaultQuota) ??
                       "2mb";
        var maxFileSizeStr =
            await SettingManager.GetOrNullForCurrentTenantAsync(FileManagementSettings.Users.DefaultMaxFileSize) ??
            "2mb";
        entity.SetStorageQuota(quotaStr.ParseToBytes());
        entity.SetMaxFileSize(maxFileSizeStr.ParseToBytes());

        await CreateAsync(entity);

        return entity;
    }

    public virtual async Task<bool> IsOwnerAsync(Folder folder, Guid userId)
    {
        var folderName = await GetUserRootFolderNameAsync(userId);
        var codeLength = FolderConsts.GetCodeLength(2);
        if (folder.Code.Length <= codeLength)
        {
            return folder.Name == folderName;
        }

        var parent = await Repository.GetAsync(m => m.Code == folder.Code.Substring(0, codeLength));
        return parent.Name == folderName;
    }

    public virtual Task<string> GetUserRootFolderNameAsync(Guid userId)
    {
        return Task.FromResult(FolderConsts.UsersRootFolderName + "_" + userId);
    }

    public override async Task<Folder> CloneAsync(Folder source)
    {
        var folder = new Folder(GuidGenerator.Create(), source.ParentId, source.Name, source.IsStatic, source.TenantId);
        folder.ChangeInheritPermissions(source.IsInheritPermissions);
        folder.SetStorageQuota(source.StorageQuota);
        folder.SetUsedStorage(source.UsedStorage);

        await DistributedEventBus.PublishAsync(
            new FolderCopyEto(source.Id, folder.Id, folder.TenantId)
        );

        return folder;
    }

    public virtual async Task MoveFilesAsync(Guid sourceFolderId, Guid destinationFolderId)
    {
        var sourceFolder = await GetAsync(sourceFolderId);
        var destinationFolder = await GetAsync(destinationFolderId);

        var files = await Repository.GetFilesAsync(sourceFolder.Id);
        foreach (var file in files)
        {
            //destinationFolder.AddFile(file.FileId);
        }

        await Repository.UpdateAsync(destinationFolder);
    }
}