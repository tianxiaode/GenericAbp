using Generic.Abp.Extensions.Trees;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using System.Threading;
using System.Collections.Generic;

namespace Generic.Abp.FileManagement.Resources;

public interface IResourceRepository : ITreeRepository<Resource, ResourceQueryOption, ResourceSearchParams>
{
    Task<Resource?> FindAsync(
        Expression<Func<Resource, bool>> predicate,
        Guid? parentId,
        ResourceQueryOption option,
        CancellationToken cancellation = default);

    Task<Resource?> FindAsync(string name, Guid? parentId, ResourceQueryOption option,
        CancellationToken cancellation = default);

    Task<Resource?> GetAsync(Guid id, Guid? parentId, ResourceQueryOption option,
        CancellationToken cancellation = default);

    Task<Resource?> GetInheritedPermissionParentAsync(Guid id, string code, CancellationToken cancellationToken);


    Task<Expression<Func<Resource, bool>>> BuildQueryExpressionAsync(
        Guid parentId,
        ResourceSearchParams search);
}