using Generic.Abp.Extensions.Extensions;
using Generic.Abp.FileManagement.Folders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Generic.Abp.FileManagement;

public class FileManagementPermissionManagerBase<TEntity, TRepository>(
    TRepository repository
) : DomainService
    where TEntity : class, IPermission, IEntity<Guid>
    where TRepository : IRepository<TEntity, Guid>
{
    protected TRepository Repository { get; } = repository;

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

    public virtual async Task<bool> ExistAsync(Guid targetId, IList<string> roles,
        Expression<Func<TEntity, bool>> subPredicate, CancellationToken cancellationToken)
    {
        return await CheckPermissionAsync(targetId, roles, subPredicate, cancellationToken);
    }

    public virtual async Task<bool> CanReadAsync(Guid targetId, IList<string> roles,
        Expression<Func<TEntity, bool>> subPredicate, CancellationToken cancellationToken)
    {
        return await CheckPermissionAsync(targetId, roles, subPredicate, cancellationToken, canRead: true);
    }

    public virtual async Task<bool> CanWriteAsync(Guid targetId, IList<string> roles,
        Expression<Func<TEntity, bool>> subPredicate, CancellationToken cancellationToken)
    {
        return await CheckPermissionAsync(targetId, roles, subPredicate, cancellationToken, canWrite: true);
    }

    public virtual async Task<bool> CanDeleteAsync(Guid targetId, IList<string> roles,
        Expression<Func<TEntity, bool>> subPredicate, CancellationToken cancellationToken)
    {
        return await CheckPermissionAsync(targetId, roles, subPredicate, cancellationToken, canDelete: true);
    }

    protected virtual async Task<bool> CheckPermissionAsync(Guid targetId, IList<string> roles,
        Expression<Func<TEntity, bool>> subPredicate,
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


        //判断是否包含用户角色
        if (roles.Count > 0)
        {
            subPredicate = subPredicate.OrIfNotTrue(m =>
                m.ProviderName == FolderConsts.RoleProviderName && string.IsNullOrEmpty(m.ProviderKey) &&
                m.ProviderKey.IsIn(roles));
        }

        predicate = predicate.AndIfNotTrue(subPredicate);
        // 权限主体判断逻辑可由子类扩展实现
        return await Repository.AnyAsync(predicate, cancellationToken: cancellationToken);
    }
}