using Generic.Abp.FileManagement.Folders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.Files;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.Folders;

public class FolderRepository(IDbContextProvider<IFileManagementDbContext> dbContextProvider)
    : EfCoreRepository<IFileManagementDbContext, Folder, Guid>(dbContextProvider), IFolderRepository
{
    public virtual async Task<bool> FilesExistAsync(Guid folderId, Guid fileId, CancellationToken cancellationToken)
    {
        var dbContext = await GetDbContextAsync();
        var dbSet = dbContext.Set<File>();
        return await dbSet.AnyAsync(m => m.FolderId == folderId && m.Id == fileId, cancellationToken);
    }

    [UnitOfWork]
    public virtual async Task<bool> HasChildAsync(Guid id, CancellationToken cancellation = default)
    {
        return await (await GetQueryableAsync()).AnyAsync(m => m.ParentId == id, GetCancellationToken(cancellation));
    }


    [UnitOfWork]
    public virtual async Task<List<string>> GetAllCodesByFilterAsync(string filter,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Where(m => EF.Functions.Like(m.Name, $"%{filter}%"))
            .Select(m => m.Code)
            .ToListAsync(cancellationToken);
    }

    [UnitOfWork]
    public virtual async Task<List<File>> GetFilesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var dbSet = dbContext.Set<File>();
        return await dbSet.AsNoTracking().Where(m => m.FolderId == id).ToListAsync(cancellationToken);
    }
}