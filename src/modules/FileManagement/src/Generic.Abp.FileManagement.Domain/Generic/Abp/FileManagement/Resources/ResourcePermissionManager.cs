using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Extensions;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;

namespace Generic.Abp.FileManagement.Resources;

public class ResourcePermissionManager(IResourcePermissionRepository repository, IdentityUserManager userManager)
    : DomainService
{
    protected IResourcePermissionRepository Repository { get; } = repository;
    protected IdentityUserManager UserManager { get; } = userManager;

    public virtual async Task<List<ResourcePermission>> GetListAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Repository.GetListAsync(m => m.ResourceId == id, cancellationToken: cancellationToken);
    }

    public virtual async Task InsertManyAsync(List<ResourcePermission> permissions, CancellationToken cancellationToken)
    {
        await Repository.InsertManyAsync(permissions, cancellationToken: cancellationToken);
    }

    public virtual async Task UpdateManyAsync(List<ResourcePermission> permissions, CancellationToken cancellationToken)
    {
        await Repository.UpdateManyAsync(permissions, cancellationToken: cancellationToken);
    }

    public virtual Task DeleteManyAsync(List<ResourcePermission> permissions, CancellationToken cancellationToken)
    {
        return Repository.DeleteManyAsync(permissions, cancellationToken: cancellationToken);
    }

    public virtual async Task<bool> AllowEveryOneReadAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            m => m.ResourceId == id && m.CanRead && m.ProviderName == ResourceConsts.EveryoneProviderName,
            cancellationToken);
    }

    public virtual async Task<bool> AllowAuthenticatedUserReadAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            m => m.ResourceId == id && m.CanRead && m.ProviderName == ResourceConsts.AuthorizationUserProviderName,
            cancellationToken);
    }

    public virtual async Task<bool> AllowUserOrRolesReadAsync(Guid id, Guid userId,
        CancellationToken cancellationToken)
    {
        return await CheckPermissionAsync(id, userId, cancellationToken);
    }

    public virtual async Task<bool> AllowEveryOneWriteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            m => m.ResourceId == id && m.CanWrite && m.ProviderName == ResourceConsts.EveryoneProviderName,
            cancellationToken);
    }

    public virtual async Task<bool> AllowAuthenticatedUserWriteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            m => m.ResourceId == id && m.CanWrite && m.ProviderName == ResourceConsts.AuthorizationUserProviderName,
            cancellationToken);
    }

    public virtual async Task<bool> AllowUserOrRolesWriteAsync(Guid id, Guid userId,
        CancellationToken cancellationToken)
    {
        return await CheckPermissionAsync(id, userId, cancellationToken, canWrite: true);
    }

    public virtual async Task<bool> AllowEveryOneDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            m => m.ResourceId == id && m.CanDelete && m.ProviderName == ResourceConsts.EveryoneProviderName,
            cancellationToken);
    }

    public virtual async Task<bool> AllowAuthenticatedUserDeleteAsync(Guid id,
        CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            m => m.ResourceId == id && m.CanDelete &&
                 m.ProviderName == ResourceConsts.AuthorizationUserProviderName,
            cancellationToken);
    }

    public virtual async Task<bool> AllowUserOrRolesDeleteAsync(Guid id, Guid userId,
        CancellationToken cancellationToken)
    {
        return await CheckPermissionAsync(id, userId, cancellationToken, canDelete: true);
    }

    public virtual async Task<bool> HasPermissionAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(m => m.ResourceId == id, cancellationToken: cancellationToken);
    }


    protected virtual async Task<bool> CheckPermissionAsync(Guid id, Guid userId,
        CancellationToken cancellationToken,
        bool canRead = false, bool canWrite = false, bool canDelete = false)
    {
        Expression<Func<ResourcePermission, bool>> predicate = m => m.ResourceId == id;

        if (canRead)
        {
            predicate = predicate.AndIfNotTrue(m => m.CanRead);
        }

        if (canWrite)
        {
            predicate = predicate.AndIfNotTrue(m => m.CanWrite);
        }

        if (canDelete)
        {
            predicate = predicate.AndIfNotTrue(m => m.CanDelete);
        }

        Expression<Func<ResourcePermission, bool>> subPredicate = m =>
            m.ProviderName == ResourceConsts.AuthorizationUserProviderName && m.ProviderKey == userId.ToString();

        //判断是否包含用户角色
        var roles = await UserManager.GetRolesAsync(await UserManager.GetByIdAsync(userId));
        if (roles.Count > 0)
        {
            subPredicate = subPredicate.OrIfNotTrue(m =>
                m.ProviderName == ResourceConsts.RoleProviderName && string.IsNullOrEmpty(m.ProviderKey) &&
                m.ProviderKey.IsIn(roles));
        }

        predicate = predicate.AndIfNotTrue(subPredicate);
        return await Repository.AnyAsync(predicate, cancellationToken: cancellationToken);
    }

    public virtual async Task<List<ResourcePermission>> GetAllParentPermissionsAsync(List<Guid> parentIds,
        CancellationToken cancellationToken)
    {
        return await Repository.GetListAsync(m => parentIds.Contains(m.ResourceId), false, cancellationToken);
    }
}