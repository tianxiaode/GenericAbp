using Generic.Abp.Extensions.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Entities.IncludeOptions;
using Generic.Abp.Extensions.Entities.QueryParams;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.Extensions.EntityFrameworkCore;

public class ExtensionRepository<TDbContext, TEntity>(IDbContextProvider<TDbContext> dbContextProvider)
    : EfCoreRepository<TDbContext, TEntity, Guid>(dbContextProvider), IExtensionRepository<TEntity>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity<Guid>
{
    public virtual async Task<TEntity> GetAsync(Guid id, IIncludeOptions includeOptions,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(m => m.Id == id, includeOptions, cancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(TEntity), id);
        }

        return entity;
    }

    public virtual async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate,
        IIncludeOptions? includeOptions = null,
        CancellationToken cancellationToken = default)
    {
        return await (await IncludeDetailsAsync(includeOptions)).FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).Where(predicate).LongCountAsync(cancellationToken);
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, string sorting,
        IIncludeOptions? includeOptions = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sorting))
        {
            throw new InvalidOperationException(
                "Sorting parameter is required but was not provided. Check the query parameters or override the default sorting pattern.");
        }

        return await (await IncludeDetailsAsync(includeOptions))
            .AsNoTracking()
            .Where(predicate)
            .OrderBy(sorting)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate,
        string sorting,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        IIncludeOptions? includeOptions = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sorting))
        {
            throw new InvalidOperationException(
                "Sorting parameter is required but was not provided. Check the query parameters or override the default sorting pattern.");
        }

        return await (await IncludeDetailsAsync(includeOptions))
            .AsNoTracking()
            .Where(predicate)
            .OrderBy(sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TEntity>> GetPagedListAsync(
        Expression<Func<TEntity, bool>> predicate,
        BaseQueryParams queryParams,
        IIncludeOptions? includeOptions = null,
        CancellationToken cancellationToken = default)
    {
        return await GetPagedListAsync(predicate, queryParams.GetSorting(), queryParams.SkipCount,
            queryParams.MaxResultCount,
            includeOptions, cancellationToken);
    }

    protected virtual async Task<IQueryable<TEntity>> IncludeDetailsAsync(IIncludeOptions? includeOptions)
    {
        return await GetQueryableAsync().ConfigureAwait(false);
    }
}