using Generic.Abp.Extensions.EntityFrameworkCore.Trees;
using Generic.Abp.MenuManagement.Menus;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.MenuManagement.EntityFrameworkCore.Menus;

public class MenuRepository(IDbContextProvider<IMenuManagementDbContext> dbContextProvider)
    : TreeRepository<IMenuManagementDbContext, Menu>(dbContextProvider), IMenuRepository;