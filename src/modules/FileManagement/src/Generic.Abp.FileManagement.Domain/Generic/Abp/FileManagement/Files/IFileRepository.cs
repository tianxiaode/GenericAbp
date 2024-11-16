using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.FileManagement.Files;

public interface IFileRepository : IRepository<File, Guid>
{
    Task<List<File>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting,
        Expression<Func<File, bool>>? predicate = null, CancellationToken cancellationToken = default);

    Task<List<File>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting, string filter,
        CancellationToken cancellationToken = default);
}