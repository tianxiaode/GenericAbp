using Generic.Abp.MenuManagement.Menus.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.MenuManagement.Menus;

[RemoteService(Name = MenuManagementRemoteServiceConsts.RemoteServiceName)]
[Area(MenuManagementRemoteServiceConsts.RemoteServiceName)]
[ControllerName("Menus")]
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

    [HttpGet]
    [Route(("by-group/{groupName}"))]
    public async Task<ListResultDto<MenuDto>> GetListByGroupAsync(string groupName)
    {
        return await AppService.GetListByGroupAsync(groupName);
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
    [Route("{id:guid}")]
    public async Task<ListResultDto<MenuDto>> DeleteAsync(Guid id)
    {
        return await AppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id:guid}/multi-lingual")]
    public async Task<Dictionary<string, object>> GetMultiLingualAsync(Guid id)
    {
        return await AppService.GetMultiLingualAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/multi-lingual")]
    public async Task UpdateMultiLingualAsync(Guid id, Dictionary<string, object> input)
    {
        await AppService.UpdateMultiLingualAsync(id, input);
    }

    [HttpGet]
    [Route("groups")]
    public async Task<ListResultDto<string>> GetAllGroupNamesAsync()
    {
        return await AppService.GetAllGroupNamesAsync();
    }

    [HttpGet]
    [Route("{id:guid}/permissions")]
    public async Task<List<string>> GetPermissionsListAsync(Guid id)
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