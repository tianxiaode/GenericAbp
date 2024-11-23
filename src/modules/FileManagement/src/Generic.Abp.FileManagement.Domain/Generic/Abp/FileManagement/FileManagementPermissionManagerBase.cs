using Generic.Abp.Extensions.Extensions;
using Generic.Abp.FileManagement.Folders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;

namespace Generic.Abp.FileManagement;

public class FileManagementPermissionManagerBase<TEntity, TRepository>(
    TRepository repository,
    IdentityUserManager userManager
) : DomainService
    where TEntity : class, IPermission, IEntity<Guid>
    where TRepository : IRepository<TEntity, Guid>
{
    protected TRepository Repository { get; } = repository;
    protected IdentityUserManager UserManager { get; } = userManager;

    public virtual async Task<List<TEntity>> GetListAsync(Guid targetId, CancellationToken cancellationToken)
    {
        return await Repository.GetListAsync(m => m.TargetId == targetId, cancellationToken: cancellationToken);
    }

    public virtual async Task InsertManyAsync(List<TEntity> permissions, CancellationToken cancellationToken)
    {
        await Repository.InsertManyAsync(permissions, cancellationToken: cancellationToken);
    }

    public virtual async Task UpdateManyAsync(List<TEntity> permissions, CancellationToken cancellationToken)
    {
        await Repository.UpdateManyAsync(permissions, cancellationToken: cancellationToken);
    }

    public virtual Task DeleteManyAsync(List<TEntity> permissions, CancellationToken cancellationToken)
    {
        return Repository.DeleteManyAsync(permissions, cancellationToken: cancellationToken);
    }

    public virtual async Task<bool> AllowEveryOneReadAsync(Guid targetId, CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            m => m.TargetId == targetId && m.CanRead && m.ProviderName == FolderConsts.EveryoneProviderName,
            cancellationToken);
    }

    public virtual async Task<bool> AllowAuthenticatedUserReadAsync(Guid targetId, CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            m => m.TargetId == targetId && m.CanRead && m.ProviderName == FolderConsts.AuthorizationUserProviderName,
            cancellationToken);
    }

    public virtual async Task<bool> AllowUserOrRolesReadAsync(Guid targetId, Guid userId,
        CancellationToken cancellationToken)
    {
        return await CheckPermissionAsync(targetId, userId, cancellationToken);
    }

    public virtual async Task<bool> AllowEveryOneWriteAsync(Guid targetId, CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            m => m.TargetId == targetId && m.CanWrite && m.ProviderName == FolderConsts.EveryoneProviderName,
            cancellationToken);
    }

    public virtual async Task<bool> AllowAuthenticatedUserWriteAsync(Guid targetId, CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            m => m.TargetId == targetId && m.CanWrite && m.ProviderName == FolderConsts.AuthorizationUserProviderName,
            cancellationToken);
    }

    public virtual async Task<bool> AllowUserOrRolesWriteAsync(Guid targetId, Guid userId,
        CancellationToken cancellationToken)
    {
        return await CheckPermissionAsync(targetId, userId, cancellationToken, canWrite: true);
    }

    public virtual async Task<bool> AllowEveryOneDeleteAsync(Guid targetId, CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            m => m.TargetId == targetId && m.CanDelete && m.ProviderName == FolderConsts.EveryoneProviderName,
            cancellationToken);
    }

    public virtual async Task<bool> AllowAuthenticatedUserDeleteAsync(Guid targetId,
        CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            m => m.TargetId == targetId && m.CanDelete && m.ProviderName == FolderConsts.AuthorizationUserProviderName,
            cancellationToken);
    }

    public virtual async Task<bool> AllowUserOrRolesDeleteAsync(Guid targetId, Guid userId,
        CancellationToken cancellationToken)
    {
        return await CheckPermissionAsync(targetId, userId, cancellationToken, canDelete: true);
    }

    public virtual async Task<bool> HasPermissionAsync(Guid targetId, CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(m => m.TargetId == targetId, cancellationToken: cancellationToken);
    }


    protected virtual async Task<bool> CheckPermissionAsync(Guid targetId, Guid userId,
        CancellationToken cancellationToken,
        bool canRead = false, bool canWrite = false, bool canDelete = false)
    {
        Expression<Func<TEntity, bool>> predicate = m => m.TargetId == targetId;

        if (canRead)
        {
            predicate = predicate.And(m => m.CanRead);
        }

        if (canWrite)
        {
            predicate = predicate.And(m => m.CanWrite);
        }

        if (canDelete)
        {
            predicate = predicate.And(m => m.CanDelete);
        }

        Expression<Func<TEntity, bool>> subPredicate = m =>
            m.ProviderName == FolderConsts.AuthorizationUserProviderName && m.ProviderKey == userId.ToString();

        //判断是否包含用户角色
        var roles = await UserManager.GetRolesAsync(await UserManager.GetByIdAsync(userId));
        if (roles.Count > 0)
        {
            subPredicate = subPredicate.OrIfNotTrue(m =>
                m.ProviderName == FolderConsts.RoleProviderName && string.IsNullOrEmpty(m.ProviderKey) &&
                m.ProviderKey.IsIn(roles));
        }

        predicate = predicate.AndIfNotTrue(subPredicate);
        return await Repository.AnyAsync(predicate, cancellationToken: cancellationToken);
    }
}