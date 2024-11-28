using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

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


    public virtual async Task<bool> CadReadAsync(Resource entity, Guid? userId)
    {
        if (!userId.HasValue)
        {
            return await PermissionManager.AllowEveryOneReadAsync(entity.Id, CancellationToken);
        }

        if (await PermissionManager.AllowAuthenticatedUserReadAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        return await PermissionManager.AllowUserOrRolesReadAsync(entity.Id, userId.Value, CancellationToken);
    }

    public virtual async Task<bool> CadWriteAsync(Resource entity, Guid userId)
    {
        if (await PermissionManager.AllowAuthenticatedUserWriteAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        return await PermissionManager.AllowUserOrRolesWriteAsync(entity.Id, userId, CancellationToken);
    }

    public virtual async Task<bool> CadDeleteAsync(Resource entity, Guid userId)
    {
        if (await PermissionManager.AllowAuthenticatedUserDeleteAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        return await PermissionManager.AllowUserOrRolesDeleteAsync(entity.Id, userId, CancellationToken);
    }

    // TODO:找出全部父级权限，然后逐级往上查找带权限的文件夹，然后将该权限作为当前资源的权限
    public virtual Task<bool> GetAllParentPermissionsAsync(Resource entity, Guid userId)
    {
        throw new NotImplementedException();
    }
}