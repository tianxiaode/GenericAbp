using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.Extensions.Trees;
using Generic.Abp.MenuManagement.Localization;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
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
        if (await Repository.AnyAsync(m =>
                m.ParentId == entity.ParentId && m.Id != entity.Id &&
                m.Name.ToLower() == entity.Name.ToLowerInvariant()))
        {
            throw new DuplicateWarningBusinessException(Localizer[nameof(Menu)], entity.Name);
        }
    }
}