using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
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
    public async Task<bool> HasChildAsync(Guid id, CancellationToken cancellation = default)
    {
        return await (await GetQueryableAsync()).AnyAsync(m => m.ParentId == id, GetCancellationToken(cancellation));
    }

    [UnitOfWork]
    public async Task<List<Menu>> GetFilterListAsync(string filter, CancellationToken cancellation = default)
    {
        if (string.IsNullOrEmpty(filter)) return new List<Menu>();
        var predicate = await BuildFilterPredicateAsync(filter);
        return await GetListAsync(predicate, true, GetCancellationToken(cancellation));
    }

    [UnitOfWork]
    public async Task<List<string>> GetCodeListAsync(string filter, CancellationToken cancellation = default)
    {
        if (string.IsNullOrEmpty(filter)) return new List<string>();
        var predicate = await BuildFilterPredicateAsync(filter);
        var dbSet = await GetDbSetAsync();
        return await dbSet.Where(predicate).Select(m => m.Code).ToListAsync(GetCancellationToken(cancellation));
    }

    [UnitOfWork]
    public async Task<List<Menu>> GetListByGroupAsync(string group, CancellationToken cancellation = default)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.Where(m => EF.Functions.Like(m.GroupName, $"${group}")).ToListAsync(cancellation);
    }

    [UnitOfWork]
    public async Task<List<string>> GetAllGroupNamesAsync(CancellationToken cancellation = default)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.Select(m => m.GroupName).Distinct().ToListAsync(cancellation);
    }

    [UnitOfWork]
    protected virtual Task<Expression<Func<Menu, bool>>> BuildFilterPredicateAsync(string filter)
    {
        Expression<Func<Menu, bool>> predicate = m => EF.Functions.Like(m.DisplayName, $"%{filter}%");

        return Task.FromResult(predicate);
    }
}