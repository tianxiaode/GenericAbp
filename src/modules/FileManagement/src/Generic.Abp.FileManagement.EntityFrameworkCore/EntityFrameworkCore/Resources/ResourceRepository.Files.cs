using Generic.Abp.FileManagement.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.Resources;

/// <summary>
/// 这是管理资源的仓储实现。
/// </summary>
public partial class ResourceRepository
{
//     protected virtual async Task<IQueryable<Guid>> GetAllowedFolderIdsAsync(
//         string publicRootFolderCode,
//         string sharedFolderCode,
//         string userFolderCode,
//         Guid userId,
//         List<string> roles)
//     {
//         var dbContext = await GetDbContextAsync();
//         var folderDbSet = dbContext.Set<Resource>();
//         var folderIds = folderDbSet.Where(m =>
//             m.Code.StartsWith(publicRootFolderCode) || m.Code.StartsWith(sharedFolderCode) ||
//             m.Code.StartsWith(userFolderCode)).Select(m => m.Id);
//         var folderPermissionsDbSet = dbContext.Set<ResourcePermission>();
//         Expression<Func<ResourcePermission, bool>> permissionsPredicate = m =>
//             m.ProviderName == FolderConsts.AuthorizationUserProviderName
//             || (m.ProviderName == FolderConsts.UserProviderName && m.ProviderName == userId.ToString());
//         if (roles.Count > 0)
//         {
//             permissionsPredicate = permissionsPredicate.Or(m =>
//                 m.ProviderName == FolderConsts.RoleProviderName &&
//                 (!string.IsNullOrEmpty(m.ProviderKey) && roles.Contains(m.ProviderKey)));
//         }
//     
//         return folderPermissionsDbSet.Where(m => folderIds.Contains(m.Id) && m.CanRead)
//             .Where(permissionsPredicate)
//             .Select(m => m.TargetId);
//     }
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
//     var fileDbSet = dbContext.Set<Resource>();
//     var fileIds = fileDbSet.Where(m => allowedFolderIds.Contains(m.FolderId)).Select(m => m.Id);
//     var filePermissionsDbSet = dbContext.Set<ResourcePermission>();
//     Expression<Func<ResourcePermission, bool>> permissionsPredicate = m =>
//         m.ProviderName == ProviderNames.AuthorizationUserProviderName.
//         || (m.ProviderName == ProviderNames.UserProviderName && m.ProviderName == userId.ToString());
//     if (roles.Count > 0)
//     {
//         permissionsPredicate = permissionsPredicate.Or(m =>
//             m.ProviderName == ProviderNames.RoleProviderName &&
//             (!string.IsNullOrEmpty(m.ProviderKey) && roles.Contains(m.ProviderKey)));
//     }
//
//     var hasPermissionsAllowedFileIds = filePermissionsDbSet.Where(m => fileIds.Contains(m.Id) && m.CanRead)
//         .Where(permissionsPredicate).Select(m => m.TargetId);
//     //找出在权限表中没有定义权限的文件Id
//     var noPermissionsAllowedFileIds = fileDbSet.Where(m => !fileIds.Contains(m.Id)).Select(m => m.Id);
//     return hasPermissionsAllowedFileIds.Concat(noPermissionsAllowedFileIds);
// }
}