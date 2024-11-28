using Generic.Abp.Extensions.Entities.Multilingual;
using Generic.Abp.Extensions.Entities.Permissions;
using Generic.Abp.MenuManagement.Menus;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Generic.Abp.MenuManagement;

public class MenuDataSeed(
    IGuidGenerator guidGenerator,
    ILogger<IMenuDataSeed> logger,
    MenuManager menuManager)
    : IMenuDataSeed
{
    protected IGuidGenerator GuidGenerator { get; } = guidGenerator;
    protected MenuManager MenuManager { get; } = menuManager;
    protected ILogger<IMenuDataSeed> Logger { get; } = logger;

    public virtual async Task SeedAsync(Guid? tenantId = null)
    {
        Logger.LogInformation("Seeding default menu items...");
        var menus = DefaultMenu.GetMenuConfigurations();
        await CreateMenuAsync(menus, null, tenantId);
        Logger.LogInformation("Default menu items has been seeded.");
    }

    protected virtual async Task CreateMenuAsync(List<MenuConfig> menus, Guid? parentId, Guid? tenantId)
    {
        foreach (var menuConfig in menus)
        {
            if (tenantId.HasValue && menuConfig.Name == DefaultMenu.Tenants)
            {
                continue;
            }

            var menu = new Menu(GuidGenerator.Create(), parentId, menuConfig.Name, tenantId, true);
            menu.SetIcon(menuConfig.Icon);
            menu.SetOrder(menuConfig.Order);
            menu.SetRouter(menuConfig.Router);
            menu.SetMultilingual(menuConfig.Translations);
            menu.SetPermissions(menuConfig.Permissions);
            await MenuManager.CreateAsync(menu, false);
            if (menuConfig.Items.Count > 0)
            {
                await CreateMenuAsync(menuConfig.Items, menu.Id, tenantId);
            }
        }
    }
}