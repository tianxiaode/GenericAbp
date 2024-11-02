﻿using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.Extensions.Trees;
using Generic.Abp.MenuManagement.Localization;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Threading;

namespace Generic.Abp.MenuManagement.Menus;

public class MenuManager : TreeManager<Menu, IMenuRepository>
{
    public MenuManager(IMenuRepository repository, [NotNull] [ItemNotNull] ITreeCodeGenerator<Menu> treeCodeGenerator,
        [NotNull] ICancellationTokenProvider cancellationTokenProvider,
        IStringLocalizer<MenuManagementResource> localizer) : base(repository, treeCodeGenerator,
        cancellationTokenProvider)
    {
        Localizer = localizer;
    }

    protected IStringLocalizer<MenuManagementResource> Localizer { get; }

    public override async Task ValidateAsync(Menu entity)
    {
        await base.ValidateAsync(entity);

        var siblings = (await FindChildrenAsync(entity.ParentId))
            .Where(m => m.Id != entity.Id)
            .ToList();

        if (siblings.Any(m => m.Name == entity.Name))
        {
            throw new DuplicateWarningBusinessException(Localizer[nameof(Menu)], entity.Name);
        }
    }
}