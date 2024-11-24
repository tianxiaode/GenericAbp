using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Extensions;

namespace Generic.Abp.FileManagement.Resources;

public static class ResourcePermissionHelper
{
    public const int CanRead = 1 << 0;
    public const int CanWrite = 1 << 1;
    public const int CanDelete = 1 << 2;
    public const string UserProviderName = "U";
    public const string RoleProviderName = "R";
    public const string AuthorizationUserProviderName = "A";
    public const string EveryoneProviderName = "E";

    public static int Combine(params int[] permissions)
    {
        return permissions.Aggregate(0, (current, permission) => current | permission);
    }

    public static bool HasPermission(int permission, params int[] requiredPermissions)
    {
        // 将所有requiredPermissions中的值通过按位或运算符组合起来
        var combinedRequiredPermission = Combine(requiredPermissions);

        // 检查用户是否有足够的权限
        return (permission & combinedRequiredPermission) == combinedRequiredPermission;
    }

    public static Task<Expression<Func<TPermissionEntity, bool>>> GetPermissionExpression<TPermissionEntity>(
        params int[] requiredPermissions)
        where TPermissionEntity : class, IResourcePermission
    {
        // 将所有requiredPermissions中的值通过按位或运算符组合起来
        var combinedRequiredPermission = Combine(requiredPermissions);
        return Task.FromResult<Expression<Func<TPermissionEntity, bool>>>(m =>
            (m.Permissions & combinedRequiredPermission) == combinedRequiredPermission);
    }

    public static Task<bool> CanReadAsync(int permission) => Task.FromResult(HasPermission(permission, CanRead));
    public static Task<bool> CanWriteAsync(int permission) => Task.FromResult(HasPermission(permission, CanWrite));

    public static Task<bool> CanDeleteAsync(int permission)
        => Task.FromResult(HasPermission(permission, CanDelete));

    // public static Task<Expression<Func<IResourcePermission, bool>>> CanReadExpressionAsync()
    //     => GetPermissionExpression<TEntity>(CanRead);
    //
    // public static Task<Expression<Func<TEntity, bool>>> CanWriteExpressionAsync<TEntity>()
    //     where TEntity : class, IResourcePermission
    //     => GetPermissionExpression<TEntity>(CanWrite);
    //
    // public static Task<Expression<Func<TEntity, bool>>> CanDeleteExpressionAsync<TEntity>()
    //     where TEntity : class, IResourcePermission
    //     => GetPermissionExpression<TEntity>(CanDelete);

    public static async Task<Expression<Func<TPermissionEntity, bool>>> GetEveryOneReadExpressionAsync<
        TPermissionEntity>(Guid id)
        where TPermissionEntity : class, IResourcePermission
    {
        return await GetPermissionExpression<TPermissionEntity>(id, [EveryoneProviderName], null, [], CanRead);
    }

    public static async Task<Expression<Func<TPermissionEntity, bool>>> GetAuthenticatedUserReadExpressionAsync<
        TPermissionEntity>(Guid id)
        where TPermissionEntity : class, IResourcePermission
    {
        return await GetPermissionExpression<TPermissionEntity>(id, [AuthorizationUserProviderName], null, [], CanRead);
    }

    public static async Task<Expression<Func<TPermissionEntity, bool>>> GetUserOrRoleReadExpressionAsync<
        TPermissionEntity>(
        Guid id, Guid? userId, IList<string> roles)
        where TPermissionEntity : class, IResourcePermission
    {
        return await GetPermissionExpression<TPermissionEntity>(id, [UserProviderName, RoleProviderName], userId,
            roles, CanRead);
    }

    public static async Task<Expression<Func<TPermissionEntity, bool>>> GetEveryOneWriteExpressionAsync<
        TPermissionEntity>(Guid id)
        where TPermissionEntity : class, IResourcePermission
    {
        return await GetPermissionExpression<TPermissionEntity>(id, [EveryoneProviderName], null, [], CanWrite);
    }

    public static async Task<Expression<Func<TPermissionEntity, bool>>> GetAuthenticatedUserWriteExpressionAsync<
        TPermissionEntity>(Guid id)
        where TPermissionEntity : class, IResourcePermission
    {
        return await GetPermissionExpression<TPermissionEntity>(id, [AuthorizationUserProviderName], null, [],
            CanWrite);
    }

    public static async Task<Expression<Func<TPermissionEntity, bool>>> GetUserOrRoleWriteExpressionAsync<
        TPermissionEntity>(
        Guid id, Guid? userId, IList<string> roles)
        where TPermissionEntity : class, IResourcePermission
    {
        return await GetPermissionExpression<TPermissionEntity>(id, [UserProviderName, RoleProviderName], userId,
            roles, CanWrite);
    }

    public static async Task<Expression<Func<TPermissionEntity, bool>>> GetEveryOneDeleteExpressionAsync<
        TPermissionEntity>(Guid id)
        where TPermissionEntity : class, IResourcePermission
    {
        return await GetPermissionExpression<TPermissionEntity>(id, [EveryoneProviderName], null, [], CanDelete);
    }

    public static async Task<Expression<Func<TPermissionEntity, bool>>> GetAuthenticatedUserDeleteExpressionAsync<
        TPermissionEntity>(Guid id)
        where TPermissionEntity : class, IResourcePermission
    {
        return await GetPermissionExpression<TPermissionEntity>(id, [AuthorizationUserProviderName], null, [],
            CanDelete);
    }

    public static async Task<Expression<Func<TPermissionEntity, bool>>> GetUserOrRoleDeleteExpressionAsync<
        TPermissionEntity>(
        Guid id, Guid? userId, IList<string> roles)
        where TPermissionEntity : class, IResourcePermission
    {
        return await GetPermissionExpression<TPermissionEntity>(id, [UserProviderName, RoleProviderName], userId,
            roles, CanDelete);
    }

    public static async Task<Expression<Func<TPermissionEntity, bool>>> GetPermissionExpression<TPermissionEntity>(
        Guid id,
        string[] providerNames, Guid? userId, IList<string> roles, params int[] requiredPermissions)
        where TPermissionEntity : class, IResourcePermission
    {
        Expression<Func<TPermissionEntity, bool>> predicate = m => m.ResourceId == id;
        predicate = PredicateBuilder.And(predicate,
            await GetPermissionExpression<TPermissionEntity>(requiredPermissions));
        Expression<Func<TPermissionEntity, bool>> subPredicate = m => true;
        providerNames.Aggregate(predicate, (current, providerName) => providerName switch
        {
            EveryoneProviderName => subPredicate =
                subPredicate.OrIfNotTrue(m => m.ProviderName == EveryoneProviderName),
            AuthorizationUserProviderName when userId.HasValue => subPredicate =
                subPredicate.OrIfNotTrue(m => m.ProviderName == AuthorizationUserProviderName),
            UserProviderName when userId.HasValue => subPredicate = subPredicate.OrIfNotTrue(m =>
                m.ProviderName == UserProviderName && m.ProviderKey == userId.Value.ToString()),
            RoleProviderName when roles.Any() => subPredicate = subPredicate.OrIfNotTrue(m =>
                m.ProviderName == RoleProviderName &&
                (!m.ProviderKey.IsNullOrEmpty() && roles.Contains(m.ProviderKey))),
            _ => current
        });

        return predicate.AndIfNotTrue(subPredicate);
    }
}