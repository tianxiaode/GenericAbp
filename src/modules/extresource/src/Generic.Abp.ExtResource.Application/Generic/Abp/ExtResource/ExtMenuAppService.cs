using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Generic.Abp.ExtResource.Dtos;
using Generic.Abp.ExtResource.Menus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;

namespace Generic.Abp.ExtResource
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
            _abpAuthorizationPolicyProvider = abpAuthorizationPolicyProvider;
            _menus = menuOptions.Value;
            _authorizationService = authorizationService;
        }

        [Authorize]
        public virtual async Task<ListResultDto<DesktopMenuItemDto>> GetDesktopMenusAsync()
        {
            Logger.LogDebug($"{JsonSerializer.Serialize(_menus.Desktop)}");
            var predicate = await BuildPredicate<DesktopMenuItem>();
            var menus = _menus.Desktop.Where(m =>
                m.ParentId == null).Where(predicate).Select(m => new DesktopMenuItemDto
            {
                Id = m.Id,
                IconCls = m.IconCls,
                LangText = m.LangText,
                ParentId = m.ParentId,
                ViewType = m.ViewType,
            }).ToList();
            foreach (var menu in menus)
            {
                menu.Children = _menus.Desktop.Where(c => c.ParentId == menu.Id).Where(predicate).OrderBy(c => c.Id)
                    .Select(c => new DesktopMenuItemDto
                    {
                        Id = c.Id,
                        Children = null,
                        IconCls = c.IconCls,
                        LangText = c.LangText,
                        Leaf = true,
                        ParentId = c.ParentId,
                        ViewType = c.ViewType,
                        Selectable = true                        
                    })
                    .ToList();
                menu.Leaf = !menu.Children.Any();
                menu.Selectable = menu.Leaf;
            }
            return new ListResultDto<DesktopMenuItemDto>(menus.OrderBy(m => m.Id).ToList());
        }


        protected virtual async Task<Func<T, bool>> BuildPredicate<T>() where T : IMenuItem
        {
            var policyNames = await _abpAuthorizationPolicyProvider.GetPoliciesNamesAsync();
            var predicate = new Func<T, bool>(m=>
               !m.RequiredPermissionNames.Any() || m.RequiredPermissionNames.
                    Any(p => policyNames.Contains(p) &&
                        _authorizationService.IsGrantedAsync(p).GetAwaiter().GetResult()));
            return predicate;

        }

        [Authorize]
        public virtual async Task<ListResultDto<PhoneMenuItemDto>> GetPhoneMenusAsync()
        {
            var predicate = await BuildPredicate<PhoneMenuItem>();
            var menus = _menus.Mobile.Where(predicate).Select(m => new PhoneMenuItemDto()
            {
                Id = m.Id,
                IconCls = m.IconCls,
                LangText = m.LangText,
                ViewType = m.ViewType,
                Category = m.Category,
                Color = m.Color
            });

            return new ListResultDto<PhoneMenuItemDto>(menus.ToList());

        }

    }
}
