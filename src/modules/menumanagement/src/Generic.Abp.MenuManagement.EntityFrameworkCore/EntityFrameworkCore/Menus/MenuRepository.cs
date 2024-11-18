using Generic.Abp.MenuManagement.Menus;
using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.MenuManagement.EntityFrameworkCore.Menus;

public class MenuRepository(IDbContextProvider<IMenuManagementDbContext> dbContextProvider)
    : EfCoreRepository<IMenuManagementDbContext, Menu, Guid>(dbContextProvider), IMenuRepository;