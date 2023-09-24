using Generic.Abp.MenuManagement.Menus.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Generic.Abp.MenuManagement.Menus;

public interface IMenuAppService : IApplicationService
{
    Task<ListResultDto<MenuDto>> GetListAsync(MenuGetListInput input);
    Task<MenuDto> GetAsync(Guid id);
    Task<MenuDto> CreateAsync(MenuCreateDto input);
    Task<MenuDto> UpdateAsync(Guid id, MenuUpdateDto input);
    Task<ListResultDto<MenuDto>> DeleteAsync(List<Guid> ids);
    Task<ListResultDto<MenuTranslationDto>> GetTranslationListAsync(Guid id);
    Task UpdateTranslationAsync(Guid id, List<MenuTranslationUpdateDto> input);
    Task<ListResultDto<string>> GetAllGroupNamesAsync();
    Task<ListResultDto<string>> GetPermissionsListAsync(Guid id);
    Task UpdatePermissionsAsync(Guid id, List<string> input);
}