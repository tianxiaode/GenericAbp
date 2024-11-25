using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.Extensions.Extensions;
using Generic.Abp.Extensions.MimeDetective;
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

namespace Generic.Abp.FileManagement.Resources;

public class ResourceManager(
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


    public override async Task DeleteAsync(Resource entity, bool autoSave = true)
    {
        await SetPermissionsAsync(entity, []);
        await base.DeleteAsync(entity, autoSave);
    }

    public override async Task ValidateAsync(Resource entity)
    {
        if (await Repository.AnyAsync(m =>
                m.ParentId == entity.ParentId && m.Id != entity.Id &&
                m.Name.ToLower() == entity.Name.ToLowerInvariant()))
        {
            throw new DuplicateWarningBusinessException(Localizer[nameof(Resource)], entity.Name);
        }
    }

    #region files

    #endregion

    #region permissions

    public virtual async Task<List<ResourcePermission>> GetPermissionsAsync(Resource entity)
    {
        return await PermissionManager.GetListAsync(entity.Id, CancellationToken);
    }

    public virtual async Task SetPermissionsAsync(Resource entity, List<ResourcePermission> permissions)
    {
        var currentPermissions = await PermissionManager.GetListAsync(entity.Id, CancellationToken);

        var currentPermissionIds = new HashSet<Guid>(currentPermissions.Select(m => m.Id));
        var newPermissionIds = new HashSet<Guid>(permissions.Select(m => m.Id));

        // 找出需要删除的权限
        var removePermissions = currentPermissions.Where(m => !newPermissionIds.Contains(m.Id)).ToList();
        await PermissionManager.DeleteManyAsync(removePermissions, CancellationToken);

        // 找出需要新增的权限
        var insertPermissions = permissions.Where(m => !currentPermissionIds.Contains(m.Id)).ToList();
        await PermissionManager.InsertManyAsync(insertPermissions, CancellationToken);

        // 找出需要更新的权限
        var updatePermissions = permissions.Where(m => currentPermissionIds.Contains(m.Id)).ToList();
        await PermissionManager.UpdateManyAsync(updatePermissions, CancellationToken);
    }


    public virtual async Task<bool> CadReadAsync(Resource entity, Guid userId)
    {
        if (await PermissionManager.AllowAuthenticatedUserReadAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        return await PermissionManager.AllowUserOrRolesReadAsync(entity.Id, userId, CancellationToken);
    }

    public virtual async Task<bool> CadWriteAsync(Resource entity, Guid userId)
    {
        if (await PermissionManager.AllowAuthenticatedUserWriteAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        return await PermissionManager.AllowUserOrRolesWriteAsync(entity.Id, userId, CancellationToken);
    }

    public virtual async Task<bool> CadDeleteAsync(Resource entity, Guid userId)
    {
        if (await PermissionManager.AllowAuthenticatedUserDeleteAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        return await PermissionManager.AllowUserOrRolesDeleteAsync(entity.Id, userId, CancellationToken);
    }

    // TODO:找出全部父级权限，然后逐级往上查找带权限的文件夹，然后将该权限作为当前资源的权限
    public virtual Task<bool> GetAllParentPermissionsAsync(Resource entity, Guid userId)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region 配额和文件大小限制

    // TODO:找出全部父级权限，然后逐级往上查找带有空间配额和文件大小限制的文件夹，然后将该限额和文件大小限制作为当前资源的限额和文件大小限制
    //TODO:在设置配额和文件大小限制的时候， 必须两个参数同时设置，否则不生效

    #endregion


    public virtual async Task<bool> IsOwnerAsync(Resource entity, Guid userId)
    {
        var folderName = await GetUserRootFolderNameAsync(userId);
        var codeLength = ResourceConsts.GetCodeLength(2);
        if (entity.Code.Length <= codeLength)
        {
            return entity.Name == folderName;
        }

        var parent = await Repository.GetAsync(m => m.Code == entity.Code.Substring(0, codeLength));
        return parent.Name == folderName;
    }


    protected override Task<Resource> CloneAsync(Resource source)
    {
        var entity = new Resource(GuidGenerator.Create(), source.Name, ResourceType.Folder, source.IsStatic,
            source.TenantId);
        entity.SetFileInfoBase(source.FileInfoBase);
        entity.SetFolderId(source.FolderId);
        return Task.FromResult(entity);
    }
}