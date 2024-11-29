using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generic.Abp.FileManagement.Resources;

public partial class ResourceManager
{
    public virtual async Task<List<ResourcePermission>> GetPermissionsAsync(Resource entity)
    {
        return await PermissionManager.GetListAsync(entity.Id, CancellationToken);
    }

    public virtual async Task SetPermissionsAsync(Resource entity, List<ResourcePermission> permissions)
    {
        var currentPermissions = await PermissionManager.GetListAsync(entity.Id, CancellationToken);

        var currentPermissionIds = new HashSet<Guid>(currentPermissions.Select(m => m.Id));
        var newPermissionIds = new HashSet<Guid>(permissions.Select(m => m.Id));

        // 找出需要删除的权限
        var removePermissions = currentPermissions.Where(m => !newPermissionIds.Contains(m.Id)).ToList();
        await PermissionManager.DeleteManyAsync(removePermissions, CancellationToken);

        // 找出需要新增的权限
        var insertPermissions = permissions.Where(m => !currentPermissionIds.Contains(m.Id)).ToList();
        await PermissionManager.InsertManyAsync(insertPermissions, CancellationToken);

        // 找出需要更新的权限
        var updatePermissions = permissions.Where(m => currentPermissionIds.Contains(m.Id)).ToList();
        await PermissionManager.UpdateManyAsync(updatePermissions, CancellationToken);
    }


    public virtual async Task<bool> CanReadAsync(Resource entity, Guid? userId)
    {
        return await PermissionManager.HasPermissionAsync(entity.Id, userId, (int)ResourcePermissionType.CanRead,
            CancellationToken);
    }

    public virtual async Task<bool> CanWriteAsync(Resource entity, Guid? userId)
    {
        return await PermissionManager.HasPermissionAsync(entity.Id, userId, (int)ResourcePermissionType.CanWrite,
            CancellationToken);
    }

    public virtual async Task<bool> CanDeleteAsync(Resource entity, Guid? userId)
    {
        return await PermissionManager.HasPermissionAsync(entity.Id, userId, (int)ResourcePermissionType.CanDelete,
            CancellationToken);
    }

    /// <summary>
    /// 检查是否有继承权限
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public virtual async Task<bool> CanReadByInheritPermissionsAsync(Resource entity, Guid? userId)
    {
        return await HasInheritPermissionAsync(entity, userId, (int)ResourcePermissionType.CanRead);
    }

    public virtual async Task<bool> CanWriteByInheritPermissionsAsync(Resource entity, Guid? userId)
    {
        return await HasInheritPermissionAsync(entity, userId, (int)ResourcePermissionType.CanWrite);
    }

    public virtual async Task<bool> CanDeleteByInheritPermissionsAsync(Resource entity, Guid? userId)
    {
        return await HasInheritPermissionAsync(entity, userId, (int)ResourcePermissionType.CanDelete);
    }

    public virtual async Task<bool> HasInheritPermissionAsync(Resource entity, Guid? userId, int requiredPermission)
    {
        var parent = await Repository.GetInheritedPermissionParentAsync(entity.Id, entity.Code, CancellationToken);
        if (parent == null)
        {
            return false;
        }

        return await PermissionManager.AllowByInheritPermissionsAsync(parent.Permissions.ToList(), userId,
            requiredPermission);
    }
}