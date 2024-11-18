using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.FileManagement.Folders;
using Generic.Abp.FileManagement.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Extensions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.Threading;

namespace Generic.Abp.FileManagement.VirtualPaths;

public class VirtualPathManager(
    IVirtualPathRepository repository,
    IStringLocalizer<FileManagementResource> localizer,
    IdentityUserManager userManager,
    ICancellationTokenProvider cancellationTokenProvider,
    VirtualPathPermissionManager permissionManager)
    : DomainService
{
    protected IStringLocalizer<FileManagementResource> Localizer { get; } = localizer;
    protected ICancellationTokenProvider CancellationTokenProvider { get; } = cancellationTokenProvider;
    protected CancellationToken CancellationToken => CancellationTokenProvider.Token;
    protected VirtualPathPermissionManager PermissionManager { get; } = permissionManager;
    protected IVirtualPathRepository Repository { get; } = repository;
    protected IdentityUserManager UserManager { get; } = userManager;

    public virtual async Task<VirtualPath> GetAsync(Guid id)
    {
        var entity = await Repository.GetAsync(m => m.Id == id, cancellationToken: CancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(VirtualPath), id);
        }

        return entity;
    }

    public virtual async Task<VirtualPath> FindByPathAsync(string path)
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
        return await CheckPermissionAsync(entity, userId, PermissionManager.CanReadAsync);
    }

    public virtual async Task<bool> CadWriteAsync(VirtualPath entity, Guid? userId = null)
    {
        return await CheckPermissionAsync(entity, userId, PermissionManager.CanWriteAsync);
    }

    public virtual async Task<bool> CadDeleteAsync(VirtualPath entity, Guid? userId = null)
    {
        return await CheckPermissionAsync(entity, userId, PermissionManager.CanDeleteAsync);
    }

    public virtual async Task<bool> CheckPermissionAsync(VirtualPath entity, Guid? userId,
        Func<Guid, IList<string>, Expression<Func<VirtualPathPermission, bool>>, System.Threading.CancellationToken,
                Task<bool>>
            permissionCheckFunc)
    {
        Expression<Func<VirtualPathPermission, bool>> subPredicate = m =>
            m.ProviderName == FolderConsts.EveryoneProviderName;
        //如果未提供用户，说明是匿名用户，直接判断是否 everyone权限
        if (!userId.HasValue)
        {
            return await permissionCheckFunc(entity.Id, [], subPredicate, CancellationToken);
        }

        //是否认证用户
        subPredicate = m => m.ProviderName == FolderConsts.AuthorizationUserProviderName;

        //结合是否包含用户
        subPredicate = subPredicate.OrIfNotTrue(m =>
            m.ProviderName == FolderConsts.UserProviderName && m.ProviderKey == userId.ToString());

        var roles = await UserManager.GetRolesAsync(await UserManager.GetByIdAsync(userId.Value));
        //再结合是否包含用户角色
        return await permissionCheckFunc(entity.Id, roles, subPredicate, CancellationToken);
    }
}