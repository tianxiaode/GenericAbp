using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Extensions;
using Generic.Abp.MenuManagement.Menus;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace Generic.Abp.MenuManagement.EntityFrameworkCore.Menus;

public class MenuRepository : EfCoreRepository<IMenuManagementDbContext, Menu, Guid>, IMenuRepository
{
    public MenuRepository(IDbContextProvider<IMenuManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    [UnitOfWork]
    public virtual async Task<bool> HasChildAsync(Guid id, CancellationToken cancellation = default)
    {
        return await (await GetQueryableAsync()).AnyAsync(m => m.ParentId == id, GetCancellationToken(cancellation));
    }

    [UnitOfWork]
    public virtual async Task<List<Menu>> GetListAsync(
        Expression<Func<Menu, bool>> predicate,
        string? sorting,
        CancellationToken cancellation = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Where(predicate)
            .OrderBy(string.IsNullOrEmpty(sorting) ? MenuConsts.GetDefaultSorting() : sorting)
            .ToListAsync(GetCancellationToken(cancellation));
    }

    [UnitOfWork]
    public virtual async Task<List<string>> GetAllGroupNamesAsync(CancellationToken cancellation = default)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.AsNoTracking().Select(m => m.GroupName).Distinct().ToListAsync(cancellation);
    }

    [UnitOfWork]
    public virtual async Task<List<string>> GetAllCodesByFilterAsync(string filter, string? groupName,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .WhereIf(!string.IsNullOrEmpty(groupName), m => m.GroupName == groupName)
            .Where(m => EF.Functions.Like(m.Name, $"%{filter}%") || EF.Functions.Like(m.Router, $"%{filter}%"))
            .Select(m => m.Code)
            .ToListAsync(cancellationToken);
    }

    [UnitOfWork]
    public virtual Task<Expression<Func<Menu, bool>>> BuildPredicateAsync(
        string? filter = null,
        string? groupName = null,
        Guid? parentId = null
    )
    {
        Expression<Func<Menu, bool>> predicate = m =>
            EF.Functions.Like(m.Name, $"%{filter}%") && m.ParentId != null;

        if (!string.IsNullOrEmpty(groupName))
        {
            predicate = predicate.AndIfNotTrue(m => m.GroupName.ToLower() == groupName.ToLowerInvariant());
        }

        if (parentId != null)
        {
            predicate = predicate.AndIfNotTrue(m => m.ParentId == parentId);
        }

        return Task.FromResult(predicate);
    }
}