using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.MenuManagement.Menus;

public interface IMenuRepository : IRepository<Menu, Guid>
{
    Task<bool> HasChildAsync(Guid id, CancellationToken cancellation = default);

    Task<List<Menu>> GetListAsync(
        Expression<Func<Menu, bool>> predicate,
        string? sorting,
        CancellationToken cancellation = default);

    Task<List<string>> GetAllCodesByFilterAsync(string filter, string? groupName,
        CancellationToken cancellationToken = default);

    Task<List<string>> GetAllGroupNamesAsync(CancellationToken cancellation = default);

    Task<Expression<Func<Menu, bool>>> BuildPredicateAsync(
        string? filter = null,
        string? groupName = null,
        Guid? parentId = null
    );
}