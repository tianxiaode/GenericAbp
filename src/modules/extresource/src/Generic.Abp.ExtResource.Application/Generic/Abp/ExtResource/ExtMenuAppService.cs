using Generic.Abp.ExtResource.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var menus = _menus.Desktop.Where(m =>m.ParentId == null).OrderBy(m=>m.Order);
            var result = new List<DesktopMenuItemDto>();
            foreach (var menu in menus)
            {
                if(!await IsGrantedAsync(menu.RequiredPermissionNames)) continue;
                var dto = new DesktopMenuItemDto(menu.Id, menu.LangText, menu.IconCls, menu.ViewType, menu.Selectable);
                result.Add(dto);
                var children = _menus.Desktop.Where(m => m.ParentId == menu.Id).ToList();
                if(!children.Any()) continue;
                dto.Children = new List<DesktopMenuItemDto>();
                foreach (var child in children.OrderBy(m=>m.Order))
                {
                    if(!await IsGrantedAsync(child.RequiredPermissionNames)) continue;
                    dto.Children.Add(new DesktopMenuItemDto(child.Id,child.LangText, child.IconCls,child.ViewType,child.Selectable,menu.Id));
                }

                dto.Leaf = !dto.Children.Any();
            }
                
            return new ListResultDto<DesktopMenuItemDto>(result.ToList());
        }


        [Authorize]
        public virtual async Task<ListResultDto<PhoneMenuItemDto>> GetPhoneMenusAsync()
        {
            var result = new List<PhoneMenuItemDto>();
            foreach (var menu in _menus.Mobile.OrderBy(m=>m.Order))
            {
                if(!await IsGrantedAsync(menu.RequiredPermissionNames)) continue;
                var dto = new PhoneMenuItemDto(menu.Id, menu.LangText, menu.IconCls, menu.ViewType, menu.Category, menu.Color);
                result.Add(dto);

            }
                
            return new ListResultDto<PhoneMenuItemDto>(result.ToList());

        }

        protected virtual async Task<bool> IsGrantedAsync(List<string> permissions)
        {
            if (permissions == null || !permissions.Any())
            {
                return true;
            }

            var isGranted = false;
            foreach (var permission in permissions)
            {
                if(!await _authorizationService.IsGrantedAsync(permission)) continue;
                isGranted = true;
                break;
            }
            return isGranted;

        }

    }
}
