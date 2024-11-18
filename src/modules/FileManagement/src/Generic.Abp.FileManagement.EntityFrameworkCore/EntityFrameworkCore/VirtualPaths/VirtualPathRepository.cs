using Generic.Abp.Extensions.Extensions;
using Generic.Abp.FileManagement.VirtualPaths;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.VirtualPaths;

public class VirtualPathRepository(IDbContextProvider<FileManagementDbContext> dbContextProvider)
    : EfCoreRepository<FileManagementDbContext, VirtualPath, Guid>(dbContextProvider), IVirtualPathRepository
{
    public virtual async Task<long> GetCountAsync(Expression<Func<VirtualPath, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).Where(predicate).LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<VirtualPath>> GetListAsync(
        Expression<Func<VirtualPath, bool>> predicate,
        string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Where(predicate)
            .OrderBy(sorting.IsNullOrWhiteSpace() ? $"{nameof(VirtualPath.CreationTime)} desc" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual Task<Expression<Func<VirtualPath, bool>>> BuildPredicateAsync(string? filter = null)
    {
        Expression<Func<VirtualPath, bool>> predicate = p => true;
        if (!string.IsNullOrEmpty(filter))
        {
            predicate = predicate.AndIfNotTrue(m =>
                EF.Functions.Like(m.Path, $"%{filter}%"));
        }

        return Task.FromResult(predicate);
    }
}