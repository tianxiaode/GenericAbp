using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.FileManagement.Files;

public interface IFileRepository : IRepository<File, Guid>
{
    Task<List<File>> GetListAsync(Expression<Func<File, bool>> predicate, string? sorting,
        int maxResultCount, int skipCount, CancellationToken cancellationToken = default);

    Task<Expression<Func<File, bool>>> BuildPredicate(
        Guid? folderId,
        string? filter,
        DateTime? startTime,
        DateTime? endTime,
        string? fileTypes,
        long? minSize,
        long? maxSize
    );
}