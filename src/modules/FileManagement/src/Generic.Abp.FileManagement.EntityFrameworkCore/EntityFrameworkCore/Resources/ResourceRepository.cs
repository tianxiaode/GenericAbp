using Generic.Abp.Extensions.EntityFrameworkCore.Trees;
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
    : TreeRepository<IFileManagementDbContext, Resource>(dbContextProvider), IResourceRepository
{
    public virtual async Task<Resource?> FindAsync(
        Expression<Func<Resource, bool>> predicate,
        Guid? parentId,
        ResourceQueryOptions options,
        CancellationToken cancellation = default)
    {
        return await (await GetDbSetAsync())
            .IncludeIf(options.IncludeParent, m => m.Parent)
            .IncludeIf(options.IncludeFile, m => m.FileInfoBase)
            .IncludeIf(options.IncludePermissions, m => m.Permissions)
            .WhereIf(parentId.HasValue, m => m.ParentId == parentId)
            .Where(predicate)
            .FirstOrDefaultAsync(cancellation);
    }


    public virtual async Task<Resource?> FindAsync(string name, Guid? parentId, ResourceQueryOptions options,
        CancellationToken cancellation = default)
    {
        return await FindAsync(m => m.Name.ToLower() == name.ToLower(), parentId, options,
            cancellation);
    }

    public virtual async Task<Resource?> GetAsync(Guid id, Guid? parentId, ResourceQueryOptions options,
        CancellationToken cancellation = default)
    {
        return await FindAsync(m => m.Id == id, parentId, options, cancellation);
    }


    public virtual async Task<Resource?> GetInheritedPermissionParentAsync(Guid id, string code,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Where(m => code.StartsWith(m.Code) && m.HasPermissions) // 匹配所有父级和当前资源
            .OrderByDescending(r => r.Code) // 从最近到最远排序
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<List<Resource>> GetListAsync(
        Expression<Func<Resource, bool>> predicate,
        ResourceSearchParams search,
        ResourceQueryOptions options,
        CancellationToken cancellation = default)
    {
        return await (await GetDbSetAsync())
            .IncludeIf(options.IncludeParent, m => m.Parent)
            .IncludeIf(options.IncludeFile, m => m.FileInfoBase)
            .IncludeIf(options.IncludePermissions, m => m.Permissions)
            .OrderBy(options.Sorting!)
            .Where(predicate)
            .PageBy(options.SkipCount, options.MaxResultCount)
            .ToListAsync(cancellation);
    }

    public virtual Task<Expression<Func<Resource, bool>>> BuildQueryExpressionAsync(
        Guid parentId,
        ResourceSearchParams search)
    {
        Expression<Func<Resource, bool>> predicate = m => m.ParentId == parentId;

        if (!string.IsNullOrEmpty(search.Filter))
        {
            predicate = predicate.And(m => m.Name.Contains(search.Filter));
        }

        if (search.StartTime.HasValue)
        {
            predicate = predicate.And(m => m.CreationTime >= search.StartTime.Value);
        }

        if (search.EndTime.HasValue)
        {
            predicate = predicate.And(m => m.CreationTime <= search.EndTime.Value);
        }

        if (search.OwnerId.HasValue)
        {
            predicate = predicate.And(m => m.OwnerId == search.OwnerId);
        }


        if (string.IsNullOrEmpty(search.FileType))
        {
            return Task.FromResult(predicate);
        }


        var types = search.FileType.Split(",");
        predicate = predicate.And(m => types.Contains(m.FileExtension));

        return Task.FromResult(predicate);
    }
}