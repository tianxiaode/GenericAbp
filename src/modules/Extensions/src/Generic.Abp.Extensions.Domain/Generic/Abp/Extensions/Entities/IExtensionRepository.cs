using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Entities.QueryOptions;
using Generic.Abp.Extensions.Entities.SearchParams;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.Extensions.Entities;

public interface IExtensionRepository<TEntity, in TQueryOptions> : IRepository<TEntity, Guid>
    where TEntity : class, IEntity<Guid>
    where TQueryOptions : QueryOption
{
    Task<long> GetCountAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate,
        string sorting, int maxResultCount = int.MaxValue, int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate,
        TQueryOptions option, CancellationToken cancellationToken = default);
}

public interface
    IExtensionRepository<TEntity, in TQueryOptions, in TSearchParams> : IExtensionRepository<TEntity, TQueryOptions>
    where TEntity : class, IEntity<Guid>
    where TSearchParams : class, ISearchParams
    where TQueryOptions : QueryOption
{
    Task<Expression<Func<TEntity, bool>>> BuildPredicateExpression(TSearchParams searchParams);
}