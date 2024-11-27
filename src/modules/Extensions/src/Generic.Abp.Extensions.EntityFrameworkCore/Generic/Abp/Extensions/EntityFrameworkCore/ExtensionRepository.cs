using Generic.Abp.Extensions.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.Extensions.EntityFrameworkCore;

public class ExtensionRepository<TDbContext, TEntity>(IDbContextProvider<TDbContext> dbContextProvider)
    : EfCoreRepository<TDbContext, TEntity, Guid>(dbContextProvider), IExtensionRepository<TEntity>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity<Guid>
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
}