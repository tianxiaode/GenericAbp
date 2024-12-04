using Generic.Abp.Extensions.Trees;
using Generic.Abp.MenuManagement.Localization;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.Threading;

namespace Generic.Abp.MenuManagement.Menus;

public class MenuManager(
    IMenuRepository repository,
    ITreeCodeGenerator<Menu> treeCodeGenerator,
    ICancellationTokenProvider cancellationTokenProvider,
    IStringLocalizer<MenuManagementResource> localizer)
    : TreeManager<Menu, IMenuRepository, MenuManagementResource>(repository, treeCodeGenerator, localizer,
        cancellationTokenProvider)
{
    protected override Task<Menu> CloneAsync(Menu source)
    {
        var newMenu = new Menu(GuidGenerator.Create(), source.ParentId, source.Name, source.TenantId);
        newMenu.SetIcon(source.Icon);
        newMenu.SetRouter(source.Router);
        newMenu.SetOrder(source.Order);
        if (source.IsEnabled)
        {
            newMenu.Enable();
        }
        else
        {
            newMenu.Disable();
        }

        return Task.FromResult(newMenu);
    }
}