using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.FileManagement.Folders;

public interface IFolderRepository : IRepository<Folder, Guid>
{
    Task<bool> FilesExistAsync(Guid folderId, Guid fileId, CancellationToken cancellationToken);
    Task<List<FolderFile>> GetFilesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> HasChildAsync(Guid id, CancellationToken cancellation = default);

    Task<List<string>> GetAllCodesByFilterAsync(string filter,
        CancellationToken cancellationToken = default);
}