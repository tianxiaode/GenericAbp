using System.Collections.Generic;
using System.Threading.Tasks;
using Generic.Abp.Domain.Extensions;
using Generic.Abp.MenuManagement.Menus;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.Uow;

namespace Generic.Abp.MenuManagement;

public class MenuDataSeed : ITransientDependency, IMenuDataSeed
{
    public MenuDataSeed(IGuidGenerator guidGenerator, IUnitOfWork currentUnitOfWork, ILogger<IMenuDataSeed> logger)
    {
        GuidGenerator = guidGenerator;
        CurrentUnitOfWork = currentUnitOfWork;
        Logger = logger;
    }

    protected IGuidGenerator GuidGenerator { get; }
    protected MenuManager MenuManager { get; } = new();
    protected IUnitOfWork CurrentUnitOfWork { get; }
    protected ILogger<IMenuDataSeed> Logger { get; }

    [UnitOfWork(true)]
    public async Task SeedAsync()
    {
        var rootDesktop = new Menu(GuidGenerator.Create(), "桌面菜单", "fa-home", "dashboard", "desktop", order: 1);
        rootDesktop.SetTranslations(new List<MenuTranslation>()
        {
            new("en", "Dashboard"),
            new("zh-Hant", "綜合看板")
        });
        await MenuManager.CreateAsync(rootDesktop);
        Logger.LogInformation($"添加综合面板菜单");

        var dashboard = new Menu(GuidGenerator.Create(), "综合看版", )
    }
}