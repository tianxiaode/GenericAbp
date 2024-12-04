using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Entities.IncludeOptions;
using Generic.Abp.Extensions.Entities.QueryParams;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.Extensions.Entities;

public interface IExtensionRepository<TEntity> : IRepository<TEntity, Guid>
    where TEntity : class, IEntity<Guid>
{
    Task<TEntity> GetAsync(Guid id,
        IIncludeOptions includeOptions,
        CancellationToken cancellationToken = default);

    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate,
        IIncludeOptions? includeOptions = null,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate,
        string sorting,
        IIncludeOptions? includeOptions = null,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetPagedListAsync(
        Expression<Func<TEntity, bool>> predicate,
        string sorting,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        IIncludeOptions? includeOptions = null,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetPagedListAsync(
        Expression<Func<TEntity, bool>> predicate,
        BaseQueryParams queryParams,
        IIncludeOptions? includeOptions = null,
        CancellationToken cancellationToken = default);
}