using Generic.Abp.MenuManagement.Menus.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Generic.Abp.MenuManagement.Menus;

public interface IMenuAppService : IApplicationService
{
    Task<MenuDto> GetAsync(Guid id);
    Task<ListResultDto<MenuDto>> GetListAsync(MenuGetListInput input);
    Task<ListResultDto<MenuDto>> GetShowListAsync(string name);
    Task<MenuDto> CreateAsync(MenuCreateDto input);
    Task<MenuDto> UpdateAsync(Guid id, MenuUpdateDto input);
    Task<ListResultDto<MenuDto>> DeleteAsync(Guid id);
    Task<Dictionary<string, object>> GetMultiLingualAsync(Guid id);
    Task UpdateMultiLingualAsync(Guid id, Dictionary<string, object> input);
    Task<List<string>> GetPermissionsListAsync(Guid id);
    Task UpdatePermissionsAsync(Guid id, List<string> input);
}