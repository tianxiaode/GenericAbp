using Generic.Abp.MenuManagement.Menus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
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
    public virtual async Task<List<string>> GetAllCodesByFilterAsync(string filter, string? groupName,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Where(m => EF.Functions.Like(m.Name, $"%{filter}%") || EF.Functions.Like(m.Router, $"%{filter}%"))
            .Select(m => m.Code)
            .ToListAsync(cancellationToken);
    }
}