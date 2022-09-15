using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.Files;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace Generic.Abp.FileManagement.EntityFrameworkCore;

public class EfCoreFileRepository: EfCoreRepository<IFileManagementDbContext,Files.File,Guid>,IFileRepository
{
    public EfCoreFileRepository(IDbContextProvider<IFileManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    [UnitOfWork]
    public virtual async Task<List<File>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting,
        Expression<Func<File, bool>> predicate = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(sorting)) sorting = $"{nameof(File.Filename)}";
        var dbSet = await GetDbSetAsync();
        var query = predicate == null ? dbSet : dbSet.Where(predicate);
        return await query.OrderBy(sorting).PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    [UnitOfWork]
    public virtual Task<List<File>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting, string filter,
        CancellationToken cancellationToken = default)
    {
        Expression<Func<File, bool>> predicate =
            string.IsNullOrEmpty(filter) ? null : m => EF.Functions.Like(m.Filename, $"%{filter}%");
        return GetPagedListAsync(skipCount, maxResultCount, sorting, predicate,
            GetCancellationToken(cancellationToken));
    }
}