using Generic.Abp.Extensions.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Entities.QueryOptions;
using Generic.Abp.Extensions.Entities.SearchParams;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.Extensions.EntityFrameworkCore;

public class ExtensionRepository<TDbContext, TEntity, TQueryOptions>(IDbContextProvider<TDbContext> dbContextProvider)
    : EfCoreRepository<TDbContext, TEntity, Guid>(dbContextProvider), IExtensionRepository<TEntity, TQueryOptions>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity<Guid>
    where TQueryOptions : QueryOption
{
    public virtual async Task<long> GetCountAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).Where(predicate).LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate,
        string sorting,
        int maxResultCount = int.MaxValue, int skipCount = 0,
        bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return await (includeDetails ? await WithDetailsAsync() : await GetDbSetAsync())
            .AsNoTracking()
            .Where(predicate)
            .OrderBy(sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate,
        TQueryOptions option, CancellationToken cancellationToken = default)
    {
        return await (await IncludeDetailsAsync(option))
            .AsNoTracking()
            .Where(predicate)
            .OrderBy(option.Sorting)
            .PageBy(option.SkipCount, option.MaxResultCount)
            .ToListAsync(cancellationToken);
    }

    protected virtual async Task<IQueryable<TEntity>> IncludeDetailsAsync(TQueryOptions option)
    {
        return await GetQueryableAsync().ConfigureAwait(false);
    }
}

public class ExtensionRepository<TDbContext, TEntity, TQueryOptions, TSearchParams>(
    IDbContextProvider<TDbContext> dbContextProvider)
    : ExtensionRepository<TDbContext, TEntity, TQueryOptions>(dbContextProvider),
        IExtensionRepository<TEntity, TQueryOptions, TSearchParams>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity<Guid>
    where TSearchParams : class, ISearchParams
    where TQueryOptions : QueryOption
{
    public virtual Task<Expression<Func<TEntity, bool>>> BuildPredicateExpression(TSearchParams searchParams)
    {
        throw new NotImplementedException();
    }
}