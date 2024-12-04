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
    : TreeRepository<IFileManagementDbContext, Resource, ResourceIncludeOptions, ResourceQueryParams>(
            dbContextProvider),
        IResourceRepository
{
    public virtual async Task<Resource?> FindAsync(
        Expression<Func<Resource, bool>> predicate,
        Guid? parentId,
        ResourceIncludeOptions option,
        CancellationToken cancellation = default)
    {
        return await (await GetDbSetAsync())
            .IncludeIf(option.IncludeParent, m => m.Parent)
            .IncludeIf(option.IncludeFile, m => m.FileInfoBase)
            .IncludeIf(option.IncludePermissions, m => m.Permissions)
            .WhereIf(parentId.HasValue, m => m.ParentId == parentId)
            .Where(predicate)
            .FirstOrDefaultAsync(cancellation);
    }


    public virtual async Task<Resource?> FindAsync(string name, Guid? parentId, ResourceIncludeOptions option,
        CancellationToken cancellation = default)
    {
        return await FindAsync(m => m.Name.ToLower() == name.ToLower(), parentId, option,
            cancellation);
    }

    public virtual async Task<Resource?> GetAsync(Guid id, Guid? parentId, ResourceIncludeOptions option,
        CancellationToken cancellation = default)
    {
        return await FindAsync(m => m.Id == id, parentId, option, cancellation);
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


    protected override async Task<IQueryable<Resource>> IncludeDetailsAsync(ResourceIncludeOptions option)
    {
        return (await base.IncludeDetailsAsync(option))
            .IncludeIf(option.IncludeParent, m => m.Parent)
            .IncludeIf(option.IncludeFile, m => m.FileInfoBase)
            .IncludeIf(option.IncludePermissions, m => m.Permissions);
    }

    public virtual Task<Expression<Func<Resource, bool>>> BuildQueryExpressionAsync(
        Guid parentId,
        ResourceQueryParams query)
    {
        Expression<Func<Resource, bool>> predicate = m => m.ParentId == parentId;

        if (!string.IsNullOrEmpty(query.Filter))
        {
            predicate = predicate.And(m => m.Name.Contains(query.Filter));
        }

        if (query.StartTime.HasValue)
        {
            predicate = predicate.And(m => m.CreationTime >= query.StartTime.Value);
        }

        if (query.EndTime.HasValue)
        {
            predicate = predicate.And(m => m.CreationTime <= query.EndTime.Value);
        }

        if (query.OwnerId.HasValue)
        {
            predicate = predicate.And(m => m.OwnerId == query.OwnerId);
        }


        if (string.IsNullOrEmpty(query.FileType))
        {
            return Task.FromResult(predicate);
        }


        var types = query.FileType.Split(",");
        predicate = predicate.And(m => types.Contains(m.FileExtension));

        return Task.FromResult(predicate);
    }
}