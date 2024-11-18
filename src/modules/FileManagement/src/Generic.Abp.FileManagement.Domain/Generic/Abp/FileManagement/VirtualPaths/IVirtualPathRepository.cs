using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using Volo.Abp.Domain.Repositories;
using System.Collections.Generic;

namespace Generic.Abp.FileManagement.VirtualPaths;

public interface IVirtualPathRepository : IRepository<VirtualPath, Guid>
{
    Task<long> GetCountAsync(Expression<Func<VirtualPath, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<List<VirtualPath>> GetListAsync(
        Expression<Func<VirtualPath, bool>> predicate,
        string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0,
        CancellationToken cancellationToken = default);

    Task<Expression<Func<VirtualPath, bool>>> BuildPredicateAsync(string? filter = null);
}