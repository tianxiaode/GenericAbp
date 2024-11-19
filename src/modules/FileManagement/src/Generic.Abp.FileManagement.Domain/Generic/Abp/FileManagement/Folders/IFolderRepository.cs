using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.Files;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.FileManagement.Folders;

public interface IFolderRepository : IRepository<Folder, Guid>
{
    Task<List<File>> GetFilesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> HasChildAsync(Guid id, CancellationToken cancellation = default);

    Task<List<string>> GetAllCodesByFilterAsync(string filter,
        CancellationToken cancellationToken = default);

    Task<List<Folder>> GetCanReadFoldersForUserAsync(
        Expression<Func<Folder, bool>> predicate,
        string publicRootFolderCode,
        string sharedFolderCode,
        string userFolderCode,
        Guid userId,
        List<string> roles,
        CancellationToken cancellationToken = default);

    Task<long> GetCanReadFilesCountForUserAsync(
        Expression<Func<File, bool>> predicate,
        string publicRootFolderCode,
        string sharedFolderCode,
        string userFolderCode,
        Guid userId,
        List<string> roles,
        CancellationToken cancellationToken = default);

    Task<List<File>> GetCanReadFilesForUserAsync(
        Expression<Func<File, bool>> predicate,
        string publicRootFolderCode,
        string sharedFolderCode,
        string userFolderCode,
        Guid userId,
        List<string> roles,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}