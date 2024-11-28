using Generic.Abp.Extensions.EntityFrameworkCore.Trees;
using Generic.Abp.FileManagement.Resources;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.Resources;

public partial class ResourceRepository(IDbContextProvider<IFileManagementDbContext> dbContextProvider)
    : TreeRepository<IFileManagementDbContext, Resource>(dbContextProvider), IResourceRepository
{
    // public virtual async Task<List<File>> GetFilesAsync(Guid id, CancellationToken cancellationToken = default)
    // {
    //     var dbContext = await GetDbContextAsync();
    //     var dbSet = dbContext.Set<File>();
    //     return await dbSet.AsNoTracking().Where(m => m.FolderId == id).ToListAsync(cancellationToken);
    // }
    //
    //
    // public virtual async Task<List<Folder>> GetCanReadFoldersForUserAsync(
    //     Expression<Func<Folder, bool>> predicate,
    //     string publicRootFolderCode,
    //     string sharedFolderCode,
    //     string userFolderCode,
    //     Guid userId,
    //     List<string> roles,
    //     CancellationToken cancellationToken = default)
    // {
    //     var dbContext = await GetDbContextAsync();
    //     var folderDbSet = dbContext.Set<Folder>();
    //     var allowedFolderIds = await GetAllowedFolderIdsAsync(publicRootFolderCode, sharedFolderCode, userFolderCode,
    //         userId, roles);
    //     return await folderDbSet
    //         .AsNoTracking()
    //         .Where(m => allowedFolderIds.Contains(m.Id)).Where(predicate)
    //         .ToListAsync(cancellationToken);
    // }
    //
    // public virtual async Task<long> GetCanReadFilesCountForUserAsync(
    //     Expression<Func<File, bool>> predicate,
    //     string publicRootFolderCode,
    //     string sharedFolderCode,
    //     string userFolderCode,
    //     Guid userId,
    //     List<string> roles,
    //     CancellationToken cancellationToken = default)
    // {
    //     var dbContext = await GetDbContextAsync();
    //     var fileDbSet = dbContext.Set<File>();
    //     var allowedFileIds = await GetAllowedFileIdsAsync(publicRootFolderCode, sharedFolderCode, userFolderCode,
    //         userId, roles);
    //     return await fileDbSet.AsNoTracking().Where(m => allowedFileIds.Contains(m.Id)).Where(predicate)
    //         .LongCountAsync(cancellationToken);
    // }
    //
    // public virtual async Task<List<File>> GetCanReadFilesForUserAsync(
    //     Expression<Func<File, bool>> predicate,
    //     string publicRootFolderCode,
    //     string sharedFolderCode,
    //     string userFolderCode,
    //     Guid userId,
    //     List<string> roles,
    //     string? sorting = null,
    //     int maxResultCount = int.MaxValue,
    //     int skipCount = 0,
    //     CancellationToken cancellationToken = default)
    // {
    //     var dbContext = await GetDbContextAsync();
    //     var fileDbSet = dbContext.Set<File>();
    //     var allowedFileIds = await GetAllowedFileIdsAsync(publicRootFolderCode, sharedFolderCode, userFolderCode,
    //         userId, roles);
    //     return await fileDbSet.AsNoTracking()
    //         .Include(m => m.FileInfoBase)
    //         .Include(m => m.Folder)
    //         .Where(m => allowedFileIds.Contains(m.Id)).Where(predicate)
    //         .OrderBy(sorting ?? "CreationTime DESC").PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    // }
    //
    // protected virtual async Task<IQueryable<Guid>> GetAllowedFolderIdsAsync(
    //     string publicRootFolderCode,
    //     string sharedFolderCode,
    //     string userFolderCode,
    //     Guid userId,
    //     List<string> roles)
    // {
    //     var dbContext = await GetDbContextAsync();
    //     var folderDbSet = dbContext.Set<Folder>();
    //     var folderIds = folderDbSet.Where(m =>
    //         m.Code.StartsWith(publicRootFolderCode) || m.Code.StartsWith(sharedFolderCode) ||
    //         m.Code.StartsWith(userFolderCode)).Select(m => m.Id);
    //     var folderPermissionsDbSet = dbContext.Set<FolderPermission>();
    //     Expression<Func<FolderPermission, bool>> permissionsPredicate = m =>
    //         m.ProviderName == FolderConsts.AuthorizationUserProviderName
    //         || (m.ProviderName == FolderConsts.UserProviderName && m.ProviderName == userId.ToString());
    //     if (roles.Count > 0)
    //     {
    //         permissionsPredicate = permissionsPredicate.Or(m =>
    //             m.ProviderName == FolderConsts.RoleProviderName &&
    //             (!string.IsNullOrEmpty(m.ProviderKey) && roles.Contains(m.ProviderKey)));
    //     }
    //
    //     return folderPermissionsDbSet.Where(m => folderIds.Contains(m.Id) && m.CanRead)
    //         .Where(permissionsPredicate)
    //         .Select(m => m.TargetId);
    // }
    //
    // protected virtual async Task<IQueryable<Guid>> GetAllowedFileIdsAsync(string publicRootFolderCode,
    //     string sharedFolderCode,
    //     string userFolderCode,
    //     Guid userId,
    //     List<string> roles)
    // {
    //     var allowedFolderIds = await GetAllowedFolderIdsAsync(publicRootFolderCode, sharedFolderCode, userFolderCode,
    //         userId, roles);
    //     var dbContext = await GetDbContextAsync();
    //     var fileDbSet = dbContext.Set<File>();
    //     var fileIds = fileDbSet.Where(m => allowedFolderIds.Contains(m.FolderId)).Select(m => m.Id);
    //     var filePermissionsDbSet = dbContext.Set<FilePermission>();
    //     Expression<Func<FilePermission, bool>> permissionsPredicate = m =>
    //         m.ProviderName == FolderConsts.AuthorizationUserProviderName
    //         || (m.ProviderName == FolderConsts.UserProviderName && m.ProviderName == userId.ToString());
    //     if (roles.Count > 0)
    //     {
    //         permissionsPredicate = permissionsPredicate.Or(m =>
    //             m.ProviderName == FolderConsts.RoleProviderName &&
    //             (!string.IsNullOrEmpty(m.ProviderKey) && roles.Contains(m.ProviderKey)));
    //     }
    //
    //     var hasPermissionsAllowedFileIds = filePermissionsDbSet.Where(m => fileIds.Contains(m.Id) && m.CanRead)
    //         .Where(permissionsPredicate).Select(m => m.TargetId);
    //     //找出在权限表中没有定义权限的文件Id
    //     var noPermissionsAllowedFileIds = fileDbSet.Where(m => !fileIds.Contains(m.Id)).Select(m => m.Id);
    //     return hasPermissionsAllowedFileIds.Concat(noPermissionsAllowedFileIds);
    // }

    public virtual Task<Expression<Func<Resource, bool>>> BuildQueryExpressionAsync(
        Guid parentId,
        string? filter,
        DateTime? startTime,
        DateTime? endTime,
        string? filetype)
    {
        Expression<Func<Resource, bool>> predicate = m => m.ParentId == parentId;

        if (!string.IsNullOrEmpty(filter))
        {
            predicate = predicate.And(m => m.Name.Contains(filter));
        }

        if (startTime.HasValue)
        {
            predicate = predicate.And(m => m.CreationTime >= startTime);
        }

        if (endTime.HasValue)
        {
            predicate = predicate.And(m => m.CreationTime <= endTime);
        }

        if (string.IsNullOrEmpty(filetype))
        {
            return Task.FromResult(predicate);
        }

        var types = filetype.Split(",");
        predicate = predicate.And(m => m.FileInfoBaseId != null && types.Contains(m.FileInfoBase!.Extension));

        return Task.FromResult(predicate);
    }
}