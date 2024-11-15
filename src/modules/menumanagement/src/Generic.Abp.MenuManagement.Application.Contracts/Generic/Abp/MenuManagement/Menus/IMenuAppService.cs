using Generic.Abp.MenuManagement.Menus.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.PermissionManagement;

namespace Generic.Abp.MenuManagement.Menus;

public interface IMenuAppService : IApplicationService
{
    Task<MenuDto> GetAsync(Guid id);
    Task<ListResultDto<MenuDto>> GetListAsync(MenuGetListInput input);
    Task<ListResultDto<MenuDto>> GetAllParentAndChildrenAsync(Guid id);
    Task<ListResultDto<MenuDto>> GetShowListAsync(string name);
    Task<MenuDto> CreateAsync(MenuCreateDto input);
    Task<MenuDto> UpdateAsync(Guid id, MenuUpdateDto input);
    Task MoveAsync(Guid id, Guid? parentId);
    Task CopyAsync(Guid id, Guid? parentId);
    Task DeleteAsync(Guid id);
    Task<Dictionary<string, object>> GetMultilingualAsync(Guid id);
    Task UpdateMultilingualAsync(Guid id, Dictionary<string, object> input);
    Task<GetPermissionListResultDto> GetPermissionsAsync(Guid id);
    Task UpdatePermissionsAsync(Guid id, MenuPermissionsUpdateDto input);
}