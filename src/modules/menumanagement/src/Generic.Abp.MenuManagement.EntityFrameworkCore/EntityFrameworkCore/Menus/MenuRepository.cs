using System.Linq;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Entities.IncludeOptions;
using Generic.Abp.Extensions.EntityFrameworkCore.Trees;
using Generic.Abp.MenuManagement.Menus;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.MenuManagement.EntityFrameworkCore.Menus;

public class MenuRepository(IDbContextProvider<IMenuManagementDbContext> dbContextProvider)
    : TreeRepository<IMenuManagementDbContext, Menu>(dbContextProvider), IMenuRepository
{
    protected override async Task<IQueryable<Menu>> IncludeDetailsAsync(IIncludeOptions? includeOptions)
    {
        var menuOptions = includeOptions as MenuIncludeOptions;
        var queryable = await base.IncludeDetailsAsync(includeOptions);
        if (menuOptions == null)
        {
            return queryable;
        }

        return queryable
            .IncludeIf(menuOptions.IncludeParent, m => m.Parent)
            .IncludeIf(menuOptions.IncludeChildren, m => m.Children);
    }
}