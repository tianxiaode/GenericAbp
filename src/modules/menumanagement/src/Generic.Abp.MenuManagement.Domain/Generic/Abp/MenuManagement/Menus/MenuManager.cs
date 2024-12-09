using System;
using System.Collections.Generic;
using System.Linq;
using Generic.Abp.Extensions.Trees;
using Generic.Abp.MenuManagement.Localization;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Exceptions;
using Volo.Abp.Domain.Repositories;
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

    protected override async Task CanMoveOrCopyAdditionalValidationAsync(List<Guid> sourceIds, Menu? target,
        bool isMove = true)
    {
        if (isMove)
        {
            if (sourceIds.Count == 1)
            {
                if (await Repository.AnyAsync(m => m.Id == sourceIds[0] && m.IsStatic))
                {
                    throw new StaticEntityCanNotBeMoved(L["Menu"], sourceIds[0]);
                }

                return;
            }

            var sourceMenus =
                await Repository.GetListAsync(m => sourceIds.Contains(m.Id) && m.IsStatic, false, CancellationToken);
            if (sourceMenus.Count > 0)
            {
                throw new StaticEntityCanNotBeMoved(L["Menu"], string.Join(",", sourceMenus.Select(m => m.Id)));
            }
        }
    }
}