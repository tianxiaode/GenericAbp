using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Extensions;
using Generic.Abp.FileManagement.Files;
using Generic.Abp.FileManagement.Folders;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.Files;

public class FileRepository(IDbContextProvider<IFileManagementDbContext> dbContextProvider)
    : EfCoreRepository<IFileManagementDbContext, File, Guid>(dbContextProvider), IFileRepository
{
    // public virtual async Task<long> GetCountAsync(
    //     Expression<Func<File, bool>> predicate, 
    //     Guid userId,
    //     List<string> roles,
    //     string publicFolderCode,
    //     string sharedFolderCode, 
    //     string personalFolderCode, CancellationToken cancellationToken = default)
    // {
    //     var dbContext = await GetDbContextAsync();
    //     var folderDbSet = dbContext.Folders;
    //     var folderPermissionsDbSet = dbContext.FolderPermissions;
    //     var folderIds = folderDbSet.Where(m =>
    //         m.Code.StartsWith(personalFolderCode + ".") || m.Code.StartsWith(sharedFolderCode + ".") ||
    //         m.Code.StartsWith(publicFolderCode + ".")).Select(m=>m.Id);
    //     var allowedFolderIds = folderPermissionsDbSet.Where(m => folderIds.Contains(m.TargetId) && m.CanRead && (
    //             (m.ProviderName == FolderConsts.AuthorizationUserProviderName)
    //             || (m.ProviderName == FolderConsts.UserProviderName && m.ProviderName == userId.ToString())
    //             || (m.ProviderName == FolderConsts.RoleProviderName && roles.Contains(m.ProviderName))))
    //         .Select(m => m.TargetId);
    //     predicate = predicate.AndIfNotTrue(m => allowedFolderIds.Contains(m.FolderId));
    //     var filePermissionDbSet = dbContext.FilePermissions;
    //     var allowedFileIds = filePermissionDbSet.Where(m=>m.
    //         
    //     )
    //     return await (await GetDbSetAsync()).CountAsync(predicate, cancellationToken);
    //         
    // }

    public virtual async Task<List<File>> GetListAsync(Expression<Func<File, bool>> predicate, string? sorting,
        int maxResultCount, int skipCount, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).AsNoTracking()
            .Where(predicate)
            .OrderBy(m => sorting ?? "CreationTime desc")
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync(cancellationToken);
    }

    public virtual Task<Expression<Func<File, bool>>> BuildPredicate(
        Guid? folderId,
        string? filter,
        DateTime? startTime,
        DateTime? endTime,
        string? fileTypes,
        long? minSize,
        long? maxSize
    )
    {
        Expression<Func<File, bool>> predicate = m => true;

        if (folderId.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.FolderId == folderId);
        }

        if (!filter.IsNullOrEmpty())
        {
            predicate = predicate.AndIfNotTrue(m => m.Filename.Contains(filter));
        }

        if (startTime.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.CreationTime >= startTime);
        }

        if (endTime.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.CreationTime <= endTime);
        }

        if (!fileTypes.IsNullOrEmpty())
        {
            var types = fileTypes.Split(',').Select(m => m.Trim().ToLower()).ToList();
            predicate = predicate.AndIfNotTrue(m => types.Contains(m.FileInfoBase.FileType));
        }

        if (minSize.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.FileInfoBase.Size >= minSize);
        }

        if (maxSize.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.FileInfoBase.Size <= maxSize);
        }

        return Task.FromResult(predicate);
    }
}