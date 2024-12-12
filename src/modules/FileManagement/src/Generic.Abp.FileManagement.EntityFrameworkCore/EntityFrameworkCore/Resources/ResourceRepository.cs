using Generic.Abp.Extensions.Entities.IncludeOptions;
using Generic.Abp.Extensions.EntityFrameworkCore.Trees;
using Generic.Abp.FileManagement.Exceptions;
using Generic.Abp.FileManagement.Resources;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.Resources;

public partial class ResourceRepository(IDbContextProvider<IFileManagementDbContext> dbContextProvider)
    : TreeRepository<IFileManagementDbContext, Resource>(
            dbContextProvider),
        IResourceRepository
{
    public virtual async Task<Resource?> GetInheritedPermissionParentAsync(Guid id, string code,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Where(m => code.StartsWith(m.Code) && m.HasPermissions) // 匹配所有父级和当前资源
            .OrderByDescending(r => r.Code) // 从最近到最远排序
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<long> SumSizeByCodeAsync(string code, List<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();
        if (ids.Count == 0)
        {
            return await dbSet.Where(m => m.Code.StartsWith(code + '.') && m.Type == ResourceType.File)
                .SumAsync(m => m.FileSize, cancellationToken) ?? 0;
        }

        // 查询条件 1：直接属于 `ids` 的文件
        var directFilesSize = await dbSet
            .Where(m => m.Code.StartsWith(code) && m.Type == ResourceType.File && ids.Contains(m.Id))
            .SumAsync(m => m.FileSize, cancellationToken) ?? 0;

        // 查询条件 2：属于 `childrenFolderCodes` 子路径的文件
        var childrenFolderCodes = await dbSet
            .Where(m => m.Code.StartsWith(code) && ids.Contains(m.Id))
            .Select(m => m.Code)
            .ToListAsync(cancellationToken);

        var childrenFilesSize = await dbSet
            .Where(m => m.Code.StartsWith(code) && m.Type == ResourceType.File)
            .Where(m => dbSet
                .Where(folder =>
                    folder.Code.StartsWith(code) && m.Type == ResourceType.Folder && ids.Contains(folder.Id))
                .Select(folder => folder.Code)
                .Any(folderCode => m.Code.StartsWith(folderCode + '.')))
            .SumAsync(m => m.FileSize, cancellationToken) ?? 0;
        // 合并两个部分的统计结果
        return directFilesSize + childrenFilesSize;
    }

    public virtual async Task<List<Resource>> GetChildrenByPermissionAsync(
        Expression<Func<Resource, bool>> predicate,
        ResourceQueryParams queryParams,
        Guid userId,
        IList<string> roles,
        ResourcePermissionType permissionType,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var permissionDbSet = dbContext.Set<ResourcePermission>();
        var resourceDbSet = dbContext.Set<Resource>();

        var query = from resource in resourceDbSet.Where(predicate)
            where permissionDbSet.Any(permission =>
                permission.ResourceId == resource.Id &&
                (
                    permission.ProviderName == ProviderNames.AuthorizationUserProviderName ||
                    (permission.ProviderName == ProviderNames.UserProviderName &&
                     permission.ProviderKey == userId.ToString()) ||
                    (permission.ProviderName == ProviderNames.RoleProviderName &&
                     roles.Contains(permission.ProviderKey))
                ) &&
                (permission.Permissions & (int)permissionType) == (int)permissionType
            )
            select resource;

        return await query.OrderBy(queryParams.GetSorting()).PageBy(queryParams.SkipCount, queryParams.MaxResultCount)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<Resource> GetParentWithConfiguration(string code,
        CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();
        var parent = await dbSet.Where(m => code.StartsWith(m.Code) && m.HasConfiguration)
            .OrderByDescending(m => m.Code).FirstOrDefaultAsync(cancellationToken);
        if (parent == null)
        {
            throw new FolderConfigurationNotSetBusinessException();
        }

        return parent;
    }


    protected override async Task<IQueryable<Resource>> IncludeDetailsAsync(IIncludeOptions? option)
    {
        var resourceOptions = option as ResourceIncludeOptions ?? ResourceIncludeOptions.Default;
        return (await base.IncludeDetailsAsync(option))
            .IncludeIf(resourceOptions.IncludeParent, m => m.Parent)
            .IncludeIf(resourceOptions.IncludeFile, m => m.FileInfoBase)
            .IncludeIf(resourceOptions.IncludePermissions, m => m.Permissions);
    }
}