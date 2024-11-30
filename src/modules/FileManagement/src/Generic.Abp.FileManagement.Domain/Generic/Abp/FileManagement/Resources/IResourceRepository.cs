using Generic.Abp.Extensions.Trees;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using System.Threading;
using System.Collections.Generic;

namespace Generic.Abp.FileManagement.Resources;

public interface IResourceRepository : ITreeRepository<Resource>
{
    Task<Resource?> FindAsync(
        Expression<Func<Resource, bool>> predicate,
        Guid? parentId,
        ResourceQueryOptions options,
        CancellationToken cancellation = default);

    Task<Resource?> FindAsync(string name, Guid? parentId, ResourceQueryOptions options,
        CancellationToken cancellation = default);

    Task<Resource?> GetAsync(Guid id, Guid? parentId, ResourceQueryOptions options,
        CancellationToken cancellation = default);

    Task<List<Resource>> GetListAsync(
        Expression<Func<Resource, bool>> predicate,
        ResourceSearchAndPagedAndSortedParams search,
        ResourceQueryOptions options,
        CancellationToken cancellation = default);

    Task<Resource?> GetInheritedPermissionParentAsync(Guid id, string code, CancellationToken cancellationToken);


    Task<Expression<Func<Resource, bool>>> BuildQueryExpressionAsync(
        Guid parentId,
        ResourceSearchAndPagedAndSortedParams search);
}