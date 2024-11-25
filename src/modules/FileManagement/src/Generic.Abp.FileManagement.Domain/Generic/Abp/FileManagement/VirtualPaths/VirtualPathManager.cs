using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.FileManagement.Localization;
using Generic.Abp.FileManagement.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Threading;

namespace Generic.Abp.FileManagement.VirtualPaths;

public class VirtualPathManager(
    IResourceRepository repository,
    ResourcePermissionManager permissionManager,
    IStringLocalizer<FileManagementResource> localizer,
    ICancellationTokenProvider cancellationTokenProvider)
    : DomainService
{
    protected IResourceRepository Repository { get; } = repository;
    protected ResourcePermissionManager PermissionManager { get; } = permissionManager;
    protected IStringLocalizer<FileManagementResource> Localizer { get; } = localizer;
    protected ICancellationTokenProvider CancellationTokenProvider { get; } = cancellationTokenProvider;
    protected CancellationToken CancellationToken => CancellationTokenProvider.Token;


    public virtual async Task<Resource> FindByPathAsync(string path)
    {
        var entity =
            await Repository.FindAsync(m => m.Path.ToLower() == path.ToLowerInvariant(), false, CancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(VirtualPath), path);
        }

        return entity;
    }

    public virtual async Task CreateAsync(VirtualPath entity, bool autoSave = true)
    {
        await ValidateAsync(entity);
        await Repository.InsertAsync(entity, autoSave, CancellationToken);
    }

    public virtual async Task UpdateAsync(VirtualPath entity, bool autoSave = true)
    {
        await ValidateAsync(entity);
        await Repository.UpdateAsync(entity, autoSave, CancellationToken);
    }

    public virtual async Task DeleteAsync(VirtualPath entity, bool autoSave = true)
    {
        await SetPermissionsAsync(entity, []);
        await Repository.DeleteAsync(entity, autoSave, CancellationToken);
    }

    public async Task ValidateAsync(VirtualPath entity)
    {
        if (await Repository.AnyAsync(m =>
                m.Path.ToLower() == entity.Path.ToLowerInvariant(), CancellationToken))
        {
            throw new DuplicateWarningBusinessException(Localizer[nameof(VirtualPath)], entity.Path);
        }
    }

    public virtual async Task<List<VirtualPathPermission>> GetPermissionsAsync(Guid id)
    {
        return await PermissionManager.GetListAsync(id, CancellationToken);
    }

    public virtual async Task SetPermissionsAsync(VirtualPath entity, List<VirtualPathPermission> permissions)
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

    public virtual async Task<bool> CadReadAsync(VirtualPath entity, Guid? userId = null)
    {
        if (await PermissionManager.AllowEveryOneReadAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        if (!userId.HasValue)
        {
            return false;
        }

        if (await PermissionManager.AllowAuthenticatedUserReadAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        return await PermissionManager.AllowUserOrRolesReadAsync(entity.Id, userId.Value, CancellationToken);
    }

    public virtual async Task<bool> CadWriteAsync(VirtualPath entity, Guid? userId = null)
    {
        if (await PermissionManager.AllowEveryOneWriteAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        if (!userId.HasValue)
        {
            return false;
        }

        if (await PermissionManager.AllowAuthenticatedUserWriteAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        return await PermissionManager.AllowUserOrRolesWriteAsync(entity.Id, userId.Value, CancellationToken);
    }

    public virtual async Task<bool> CadDeleteAsync(VirtualPath entity, Guid? userId = null)
    {
        if (await PermissionManager.AllowEveryOneDeleteAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        if (!userId.HasValue)
        {
            return false;
        }

        if (await PermissionManager.AllowAuthenticatedUserDeleteAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        return await PermissionManager.AllowUserOrRolesDeleteAsync(entity.Id, userId.Value, CancellationToken);
    }
}