using Generic.Abp.Extensions.Exceptions;
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

    protected override Task CanMoveOrCopyAdditionalValidationAsync(Menu source, Menu? target,
        bool isMove = true)
    {
        if (isMove && source.IsStatic)
        {
            throw new StaticEntityCanNotBeUpdatedOrDeletedBusinessException(L["Menu"], source.Name);
        }

        return Task.CompletedTask;
    }

    protected override Task<bool> BeforeDeleteAsync(Menu entity)
    {
        if (entity.IsStatic)
        {
            throw new StaticEntityCanNotBeUpdatedOrDeletedBusinessException(L["Menu"], entity.Name);
        }

        return Task.FromResult(true);
    }
}