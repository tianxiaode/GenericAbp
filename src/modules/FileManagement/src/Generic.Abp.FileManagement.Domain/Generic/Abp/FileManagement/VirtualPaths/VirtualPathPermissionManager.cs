using Generic.Abp.Extensions.Extensions;
using Generic.Abp.FileManagement.Folders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.FileManagement.VirtualPaths;

public class VirtualPathPermissionManager(IVirtualPathPermissionRepository repository)
    : FileManagementPermissionManagerBase<VirtualPathPermission, IVirtualPathPermissionRepository>(repository)
{
    public virtual async Task<bool> CheckFolderPermissionAsync(Guid virtualPathId, Guid? userId, IList<string> roles,
        bool canRead = false,
        bool canWrite = false,
        bool canDelete = false)
    {
        Expression<Func<VirtualPathPermission, bool>> predicate = m => m.TargetId == virtualPathId;
        if (canRead)
        {
            predicate.And(m => m.CanRead);
        }

        if (canWrite)
        {
            predicate.And(m => m.CanWrite);
        }

        if (canDelete)
        {
            predicate.And(m => m.CanDelete);
        }

        Expression<Func<VirtualPathPermission, bool>> subPredicate = m =>
            m.ProviderName == FolderConsts.EveryoneProviderName;

        //判断文件夹是否存在认证用户权限
        if (userId.HasValue)
        {
            subPredicate.Or(m =>
                m.ProviderName == FolderConsts.AuthorizationUserProviderName);

            //判断是否包含用户
            subPredicate.Or(m =>
                m.ProviderName == FolderConsts.UserProviderName && m.ProviderKey == userId.ToString());
        }

        //判断是否包含用户角色
        if (roles.Count > 0)
        {
            subPredicate.Or(m =>
                m.ProviderName == FolderConsts.RoleProviderName && m.ProviderKey.IsIn(roles));
        }

        return await Repository.AnyAsync(predicate.And(subPredicate));
    }
}