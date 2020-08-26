using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Generic.Abp.ExtMenu.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;

namespace Generic.Abp.ExtMenu
{
    public class ExtMenuAppService : ApplicationService, IExtMenuAppService
    {
        private readonly MenuOptions _menus;
        private readonly IAbpAuthorizationPolicyProvider _abpAuthorizationPolicyProvider;
        private readonly IAuthorizationService _authorizationService;
        public ExtMenuAppService(IOptions<MenuOptions> menuOptions,
            IAbpAuthorizationPolicyProvider abpAuthorizationPolicyProvider,
            IAuthorizationService authorizationService
            )
        {
            LocalizationResource = typeof(ExtMenuResource);
            _abpAuthorizationPolicyProvider = abpAuthorizationPolicyProvider;
            _menus = menuOptions.Value;
            _authorizationService = authorizationService;
        }

        [Authorize]
        public async Task<List<IMenuItemBaseDto>> GetMenusAsync(bool isPhone = false)
        {
            Logger.LogInformation($"{CurrentUser.IsAuthenticated}");
            return isPhone ? await GetPhoneMenusAsync() : await GetDesktopMenusAsync();
        }

        protected virtual async Task<List<IMenuItemBaseDto>> GetDesktopMenusAsync()
        {
            var predicate = await BuildPredicate<DesktopMenuItem>();
            var menus = _menus.Desktop.Where(m =>
                m.ParentId == null).Where(predicate).Select(m => new DesktopMenuItemDto
                {
                    Id = m.Id,
                    IconCls = m.IconCls,
                    IsActive = m.IsActive,
                    LangText = m.LangText,
                    ParentId = m.ParentId,
                    Selectable = m.Selectable,
                    ViewType = m.ViewType
                }).ToList();
            foreach (var menu in menus)
            {
                menu.Children = _menus.Desktop.Where(c => c.ParentId == menu.Id).Where(predicate).OrderBy(c => c.Id)
                    .Select(c => new DesktopMenuItemDto
                    {
                        Id = c.Id,
                        Children = null,
                        IconCls = c.IconCls,
                        IsActive = c.IsActive,
                        LangText = c.LangText,
                        Leaf = true,
                        ParentId = c.ParentId,
                        Selectable = c.Selectable,
                        ViewType = c.ViewType
                    })
                    .ToList();
                menu.Leaf = !menu.Children.Any();
            }
            return await Task.FromResult(new List<IMenuItemBaseDto>(menus.OrderBy(m => m.Id).ToList()));


        }


        protected virtual async Task<Func<T, bool>> BuildPredicate<T>() where T : IMenuItemBase
        {
            var policyNames = await _abpAuthorizationPolicyProvider.GetPoliciesNamesAsync();
            var predicate = new Func<T, bool>(m =>
               !m.RequiredPermissionNames.Any() || m.RequiredPermissionNames.
                    Any(p => policyNames.Contains(p) &&
                        _authorizationService.IsGrantedAsync(p).GetAwaiter().GetResult()));
            return predicate;

        }

        protected virtual async Task<List<IMenuItemBaseDto>> GetPhoneMenusAsync()
        {
            var predicate = await BuildPredicate<PhoneMenuItem>();
            var menus = _menus.Mobile.Where(predicate).Select(m => new PhoneMenuItem
            {
                Id = m.Id,
                IconCls = m.IconCls,
                LangText = m.LangText,
                ViewType = m.ViewType,
                Category = m.Category,
                Color = m.Color
            });

            return await Task.FromResult(new List<IMenuItemBaseDto>(menus));

        }

    }
}
