using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

    public virtual async Task<bool> HasPermissionAsync(
        Guid id,
        Guid? userId,
        int permission,
        CancellationToken cancellationToken = default)
    {
        // Everyone permissions
        if (!userId.HasValue)
        {
            return await CheckPermissionAsync(id, ProviderNames.EveryoneProviderName, permission, null,
                cancellationToken);
        }

        // Authenticated user permissions
        if (await CheckPermissionAsync(id, ProviderNames.AuthorizationUserProviderName, permission, null,
                cancellationToken))
        {
            return true;
        }

        // User-specific permissions
        if (await CheckPermissionAsync(id, ProviderNames.UserProviderName, permission, [userId.Value.ToString()],
                cancellationToken))
        {
            return true;
        }

        // Role-based permissions
        return await CheckRolePermissionAsync(id, userId.Value, permission, cancellationToken);
    }

    public virtual async Task<bool> CheckPermissionAsync(
        Guid id,
        string providerName,
        int permission,
        IList<string>? providerKeys = null,
        CancellationToken cancellationToken = default)
    {
        return await Repository.AnyAsync(await HasPermission(id, providerName, permission, providerKeys),
            cancellationToken);
    }

    public virtual async Task<bool> CheckRolePermissionAsync(
        Guid id,
        Guid userId,
        int permission,
        CancellationToken cancellationToken = default)
    {
        var roles = await GetRolesAsync(userId);
        return await CheckPermissionAsync(id, ProviderNames.RoleProviderName, permission, roles, cancellationToken);
    }


    public virtual async Task<bool> AllowByInheritPermissionsAsync(
        List<ResourcePermission> permissions,
        Guid? userId,
        int requiredPermission)
    {
        // Everyone permissions
        if (!userId.HasValue)
        {
            return await HasPermission(permissions, ProviderNames.EveryoneProviderName, requiredPermission);
        }

        // Authenticated users
        if (await HasPermission(permissions, ProviderNames.AuthorizationUserProviderName, requiredPermission))
        {
            return true;
        }

        // User-specific permissions
        if (await HasPermission(permissions, ProviderNames.UserProviderName, requiredPermission,
                [userId.Value.ToString()]))
        {
            return true;
        }

        // Role-based permissions
        var roles = await GetRolesAsync(userId.Value);
        return await HasPermission(permissions, ProviderNames.RoleProviderName, requiredPermission, roles);
    }

    public virtual async Task<IList<string>> GetRolesAsync(Guid userId)
    {
        return await UserManager.GetRolesAsync(await UserManager.GetByIdAsync(userId));
    }

    protected virtual Task<Expression<Func<ResourcePermission, bool>>> HasPermission(Guid id, string providerName,
        int permission, IList<string>? providerKeys = null)
    {
        Expression<Func<ResourcePermission, bool>> predicate = providerKeys switch
        {
            null => m => m.ResourceId == id &&
                         m.ProviderName == providerName &&
                         (m.Permissions & permission) == permission,
            { Count: 1 } => m => m.ResourceId == id &&
                                 m.ProviderName == providerName &&
                                 m.ProviderKey == providerKeys[0] &&
                                 (m.Permissions & permission) == permission,
            _ => m => m.ResourceId == id &&
                      m.ProviderName == providerName &&
                      !m.ProviderKey.IsNullOrEmpty() &&
                      providerKeys.Contains(m.ProviderKey) &&
                      (m.Permissions & permission) == permission
        };
        return Task.FromResult(predicate);
    }

    protected virtual Task<bool> HasPermission(
        List<ResourcePermission> permissions, string providerName,
        int permission, IList<string>? providerKeys = null)
    {
        return Task.FromResult(providerKeys switch
        {
            null => permissions.Any(m => m.ProviderName == providerName && (m.Permissions & permission) == permission),
            { Count: 1 } => permissions.Any(m =>
                m.ProviderName == providerName && m.ProviderKey == providerKeys[0] &&
                (m.Permissions & permission) == permission),
            _ => permissions.Any(m =>
                m.ProviderName == providerName && !m.ProviderKey.IsNullOrEmpty() &&
                providerKeys.Contains(m.ProviderKey) && (m.Permissions & permission) == permission)
        });
    }
}