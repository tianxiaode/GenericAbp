using Generic.Abp.Domain.Extensions;
using Generic.Abp.MenuManagement.Menus;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace Generic.Abp.MenuManagement;

public class MenuDataSeed : ITransientDependency, IMenuDataSeed
{
    public MenuDataSeed(IGuidGenerator guidGenerator, IUnitOfWork currentUnitOfWork, ILogger<IMenuDataSeed> logger,
        MenuManager menuManager)
    {
        GuidGenerator = guidGenerator;
        CurrentUnitOfWork = currentUnitOfWork;
        Logger = logger;
        MenuManager = menuManager;
    }

    protected IGuidGenerator GuidGenerator { get; }
    protected MenuManager MenuManager { get; }
    protected IUnitOfWork CurrentUnitOfWork { get; }
    protected ILogger<IMenuDataSeed> Logger { get; }

    [UnitOfWork(true)]
    public async Task SeedAsync()
    {
        var root = new Menu(GuidGenerator.Create())
        {
            DisplayName = "root",
            Order = 1,
        };

        const string desktopGroupName = "desktop";

        await MenuManager.CreateAsync(root);

        var dashboard = new Menu(GuidGenerator.Create())
        {
            ParentId = root.Id,
            DisplayName = "综合看板",
            Order = 1,
            Icon = "fa-home",
            Router = "dashboard",
            GroupName = desktopGroupName
        };
        ;
        dashboard.SetTranslations(new List<MenuTranslation>()
        {
            new("en", "Dashboard"),
            new("zh-Hant", "綜合看板")
        });

        Logger.LogInformation($"添加综合面板菜单：{System.Text.Json.JsonSerializer.Serialize(dashboard)}");
        await MenuManager.CreateAsync(dashboard);

        var systemsMaintenance = new Menu(GuidGenerator.Create())
        {
            ParentId = root.Id,
            DisplayName = "系统维护",
            Order = 2,
            Icon = "fa-cogs",
            IsSelectable = false,
            GroupName = desktopGroupName
        };

        await MenuManager.CreateAsync(systemsMaintenance);

        var users = new Menu(GuidGenerator.Create())
        {
            ParentId = systemsMaintenance.Id,
            DisplayName = "用户管理",
            Order = 3,
            Icon = "fa-users",
            Router = "users",
            GroupName = desktopGroupName
        };

        await MenuManager.CreateAsync(users);

        var roles = new Menu(GuidGenerator.Create())
        {
            ParentId = systemsMaintenance.Id,
            DisplayName = "角色管理",
            Order = 4,
            Icon = "fa-user-shield",
            Router = "roles",
            GroupName = desktopGroupName
        };

        await MenuManager.CreateAsync(roles);

        var menus = new Menu(GuidGenerator.Create())
        {
            ParentId = systemsMaintenance.Id,
            DisplayName = "菜单管理",
            Order = 5,
            Icon = "fa-list",
            Router = "menus",
            GroupName = desktopGroupName
        };

        await MenuManager.CreateAsync(menus);

        var infrastructure = new Menu(GuidGenerator.Create())
        {
            ParentId = systemsMaintenance.Id,
            DisplayName = "基础设施",
            Order = 6,
            Icon = "fa-hdd",
            Router = "infrastructure",
            GroupName = desktopGroupName
        };

        await MenuManager.CreateAsync(infrastructure);
    }
}