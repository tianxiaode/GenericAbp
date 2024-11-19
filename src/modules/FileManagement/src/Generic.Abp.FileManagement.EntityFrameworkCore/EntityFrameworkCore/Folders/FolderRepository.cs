using Generic.Abp.FileManagement.Folders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
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
    public virtual async Task<bool> HasChildAsync(Guid id, CancellationToken cancellation = default)
    {
        return await (await GetQueryableAsync()).AnyAsync(m => m.ParentId == id, GetCancellationToken(cancellation));
    }


    public virtual async Task<List<string>> GetAllCodesByFilterAsync(string filter,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Where(m => EF.Functions.Like(m.Name, $"%{filter}%"))
            .Select(m => m.Code)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<File>> GetFilesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var dbSet = dbContext.Set<File>();
        return await dbSet.AsNoTracking().Where(m => m.FolderId == id).ToListAsync(cancellationToken);
    }


    public virtual async Task<List<Folder>> GetCanReadFoldersForUserAsync(
        Expression<Func<Folder, bool>> predicate,
        string publicRootFolderCode,
        string sharedFolderCode,
        string userFolderCode,
        Guid userId,
        List<string> roles,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var folderDbSet = dbContext.Set<Folder>();
        var allowedFolderIds = await GetAllowedFolderIdsAsync(publicRootFolderCode, sharedFolderCode, userFolderCode,
            userId, roles);
        return await folderDbSet.Where(m => allowedFolderIds.Contains(m.Id)).Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCanReadFilesCountForUserAsync(
        Expression<Func<File, bool>> predicate,
        string publicRootFolderCode,
        string sharedFolderCode,
        string userFolderCode,
        Guid userId,
        List<string> roles,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var fileDbSet = dbContext.Set<File>();
        var allowedFileIds = await GetAllowedFileIdsAsync(publicRootFolderCode, sharedFolderCode, userFolderCode,
            userId, roles);
        return await fileDbSet.AsNoTracking().Where(m => allowedFileIds.Contains(m.Id)).Where(predicate)
            .LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<File>> GetCanReadFilesForUserAsync(
        Expression<Func<File, bool>> predicate,
        string publicRootFolderCode,
        string sharedFolderCode,
        string userFolderCode,
        Guid userId,
        List<string> roles,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var fileDbSet = dbContext.Set<File>();
        var allowedFileIds = await GetAllowedFileIdsAsync(publicRootFolderCode, sharedFolderCode, userFolderCode,
            userId, roles);
        return await fileDbSet.AsNoTracking().Where(m => allowedFileIds.Contains(m.Id)).Where(predicate)
            .OrderBy(sorting ?? "CreationTime DESC").PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    protected virtual async Task<IQueryable<Guid>> GetAllowedFolderIdsAsync(
        string publicRootFolderCode,
        string sharedFolderCode,
        string userFolderCode,
        Guid userId,
        List<string> roles)
    {
        var dbContext = await GetDbContextAsync();
        var folderDbSet = dbContext.Set<Folder>();
        var folderIds = folderDbSet.Where(m =>
            m.Code.StartsWith(publicRootFolderCode) || m.Code.StartsWith(sharedFolderCode) ||
            m.Code.StartsWith(userFolderCode)).Select(m => m.Id);
        var folderPermissionsDbSet = dbContext.Set<FolderPermission>();
        Expression<Func<FolderPermission, bool>> permissionsPredicate = m =>
            m.ProviderName == FolderConsts.AuthorizationUserProviderName
            || (m.ProviderName == FolderConsts.UserProviderName && m.ProviderName == userId.ToString());
        if (roles.Count > 0)
        {
            permissionsPredicate = permissionsPredicate.Or(m =>
                m.ProviderName == FolderConsts.RoleProviderName &&
                (!string.IsNullOrEmpty(m.ProviderKey) && roles.Contains(m.ProviderKey)));
        }

        return folderPermissionsDbSet.Where(m => folderIds.Contains(m.Id) && m.CanRead)
            .Where(permissionsPredicate)
            .Select(m => m.TargetId);
    }

    protected virtual async Task<IQueryable<Guid>> GetAllowedFileIdsAsync(string publicRootFolderCode,
        string sharedFolderCode,
        string userFolderCode,
        Guid userId,
        List<string> roles)
    {
        var allowedFolderIds = await GetAllowedFolderIdsAsync(publicRootFolderCode, sharedFolderCode, userFolderCode,
            userId, roles);
        var dbContext = await GetDbContextAsync();
        var fileDbSet = dbContext.Set<File>();
        var fileIds = fileDbSet.Where(m => allowedFolderIds.Contains(m.FolderId)).Select(m => m.Id);
        var filePermissionsDbSet = dbContext.Set<FilePermission>();
        Expression<Func<FilePermission, bool>> permissionsPredicate = m =>
            m.ProviderName == FolderConsts.AuthorizationUserProviderName
            || (m.ProviderName == FolderConsts.UserProviderName && m.ProviderName == userId.ToString());
        if (roles.Count > 0)
        {
            permissionsPredicate = permissionsPredicate.Or(m =>
                m.ProviderName == FolderConsts.RoleProviderName &&
                (!string.IsNullOrEmpty(m.ProviderKey) && roles.Contains(m.ProviderKey)));
        }

        var hasPermissionsAllowedFileIds = filePermissionsDbSet.Where(m => fileIds.Contains(m.Id) && m.CanRead)
            .Where(permissionsPredicate).Select(m => m.TargetId);
        //找出在权限表中没有定义权限的文件Id
        var noPermissionsAllowedFileIds = fileDbSet.Where(m => !fileIds.Contains(m.Id)).Select(m => m.Id);
        return hasPermissionsAllowedFileIds.Concat(noPermissionsAllowedFileIds);
    }
}