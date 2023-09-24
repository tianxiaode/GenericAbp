using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Generic.Abp.MenuManagement.Menus.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.MenuManagement.Menus;

[RemoteService(Name = MenuManagementRemoteServiceConsts.RemoteServiceName)]
[Area("menus")]
[Route("api/menus")]
public class MenuController : MenuManagementController, IMenuAppService
{
    public MenuController(IMenuAppService appService)
    {
        AppService = appService;
    }

    protected IMenuAppService AppService { get; }

    [HttpGet]
    public Task<ListResultDto<MenuDto>> GetListAsync(MenuGetListInput input)
    {
        return AppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<MenuDto> GetAsync(Guid id)
    {
        return await AppService.GetAsync(id);
    }

    [HttpPost]
    public async Task<MenuDto> CreateAsync([FromBody] MenuCreateDto input)
    {
        return await AppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<MenuDto> UpdateAsync(Guid id, [FromBody] MenuUpdateDto input)
    {
        return await AppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public async Task<ListResultDto<MenuDto>> DeleteAsync([FromBody] List<Guid> ids)
    {
        return await AppService.DeleteAsync(ids);
    }

    [HttpGet]
    [Route("{id:guid}/translations")]
    public async Task<ListResultDto<MenuTranslationDto>> GetTranslationListAsync(Guid id)
    {
        return await AppService.GetTranslationListAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/translations")]
    public async Task UpdateTranslationAsync(Guid id, [FromBody] List<MenuTranslationUpdateDto> input)
    {
        await AppService.UpdateTranslationAsync(id, input);
    }

    [HttpGet]
    [Route("groups")]
    public async Task<ListResultDto<string>> GetAllGroupNamesAsync()
    {
        return await AppService.GetAllGroupNamesAsync();
    }

    [HttpGet]
    [Route("{id:guid}/permissions")]
    public async Task<ListResultDto<string>> GetPermissionsListAsync(Guid id)
    {
        return await AppService.GetPermissionsListAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/permissions")]
    public async Task UpdatePermissionsAsync(Guid id, [FromBody] List<string> input)
    {
        await AppService.UpdatePermissionsAsync(id, input);
    }
}