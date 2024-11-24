using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;

namespace Generic.Abp.FileManagement.Resources;

public class ResourcePermissionManager(IResourcePermissionRepository repository, IdentityUserManager userManager)
    : DomainService
{
    protected IResourcePermissionRepository Repository { get; } = repository;
    protected IdentityUserManager UserManager { get; } = userManager;

    public virtual async Task<List<ResourcePermission>> GetListAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Repository.GetListAsync(m => m.ResourceId == id, cancellationToken: cancellationToken);
    }

    public virtual async Task InsertManyAsync(List<ResourcePermission> permissions, CancellationToken cancellationToken)
    {
        await Repository.InsertManyAsync(permissions, cancellationToken: cancellationToken);
    }

    public virtual async Task UpdateManyAsync(List<ResourcePermission> permissions, CancellationToken cancellationToken)
    {
        await Repository.UpdateManyAsync(permissions, cancellationToken: cancellationToken);
    }

    public virtual Task DeleteManyAsync(List<ResourcePermission> permissions, CancellationToken cancellationToken)
    {
        return Repository.DeleteManyAsync(permissions, cancellationToken: cancellationToken);
    }

    public virtual async Task<bool> AllowEveryOneReadAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            await ResourcePermissionHelper.GetEveryOneReadExpressionAsync<ResourcePermission>(id),
            cancellationToken);
    }

    public virtual async Task<bool> AllowAuthenticatedUserReadAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            await ResourcePermissionHelper.GetAuthenticatedUserReadExpressionAsync<ResourcePermission>(id),
            cancellationToken);
    }

    public virtual async Task<bool> AllowUserOrRolesReadAsync(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            await ResourcePermissionHelper.GetUserOrRoleReadExpressionAsync<ResourcePermission>(id, userId,
                await GetRolesAsync(userId)),
            cancellationToken);
    }

    public virtual async Task<bool> AllowEveryOneWriteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            await ResourcePermissionHelper.GetEveryOneWriteExpressionAsync<ResourcePermission>(id),
            cancellationToken);
    }

    public virtual async Task<bool> AllowAuthenticatedUserWriteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            await ResourcePermissionHelper.GetAuthenticatedUserWriteExpressionAsync<ResourcePermission>(id),
            cancellationToken);
    }

    public virtual async Task<bool> AllowUserOrRolesWriteAsync(Guid id, Guid userId,
        CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            await ResourcePermissionHelper.GetUserOrRoleWriteExpressionAsync<ResourcePermission>(id, userId,
                await GetRolesAsync(userId)),
            cancellationToken);
    }

    public virtual async Task<bool> AllowEveryOneDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            await ResourcePermissionHelper.GetEveryOneDeleteExpressionAsync<ResourcePermission>(id),
            cancellationToken);
    }

    public virtual async Task<bool> AllowAuthenticatedUserDeleteAsync(Guid id,
        CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            await ResourcePermissionHelper.GetAuthenticatedUserDeleteExpressionAsync<ResourcePermission>(id),
            cancellationToken);
    }

    public virtual async Task<bool> AllowUserOrRolesDeleteAsync(Guid id, Guid userId,
        CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(
            await ResourcePermissionHelper.GetUserOrRoleDeleteExpressionAsync<ResourcePermission>(id, userId,
                await GetRolesAsync(userId)),
            cancellationToken);
    }

    public virtual async Task<bool> HasPermissionAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Repository.AnyAsync(m => m.ResourceId == id, cancellationToken: cancellationToken);
    }


    protected virtual async Task<IList<string>> GetRolesAsync(Guid userId)
    {
        return await UserManager.GetRolesAsync(await UserManager.GetByIdAsync(userId));
    }

    public virtual async Task<List<ResourcePermission>> GetAllParentPermissionsAsync(List<Guid> parentIds,
        CancellationToken cancellationToken)
    {
        return await Repository.GetListAsync(m => parentIds.Contains(m.ResourceId), false, cancellationToken);
    }
}