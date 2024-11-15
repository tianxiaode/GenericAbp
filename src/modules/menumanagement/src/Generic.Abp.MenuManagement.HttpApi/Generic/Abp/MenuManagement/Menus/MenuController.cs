using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Generic.Abp.MenuManagement.Menus.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.PermissionManagement;

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
    [Route("{id:guid}/all-parent-and-children")]
    public virtual async Task<ListResultDto<MenuDto>> GetAllParentAndChildrenAsync(Guid id)
    {
        return await AppService.GetAllParentAndChildrenAsync(id);
    }

    [HttpGet]
    [Route(("show/{name}"))]
    public async Task<ListResultDto<MenuDto>> GetShowListAsync(string name)
    {
        return await AppService.GetShowListAsync(name);
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

    [HttpPut]
    [Route("{id:guid}/move/{parentId:guid}")]
    public async Task MoveAsync(Guid id, Guid? parentId)
    {
        await AppService.MoveAsync(id, parentId);
    }

    [HttpPut]
    [Route("{id:guid}/copy/{parentId:guid}")]
    public async Task CopyAsync(Guid id, Guid? parentId)
    {
        await AppService.CopyAsync(id, parentId);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task DeleteAsync(Guid id)
    {
        await AppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id:guid}/multilingual")]
    public async Task<Dictionary<string, object>> GetMultilingualAsync(Guid id)
    {
        return await AppService.GetMultilingualAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/multilingual")]
    public async Task UpdateMultilingualAsync(Guid id, Dictionary<string, object> input)
    {
        await AppService.UpdateMultilingualAsync(id, input);
    }


    [HttpGet]
    [Route("{id:guid}/permissions")]
    public async Task<GetPermissionListResultDto> GetPermissionsAsync(Guid id)
    {
        return await AppService.GetPermissionsAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/permissions")]
    public async Task UpdatePermissionsAsync(Guid id, [FromBody] MenuPermissionsUpdateDto input)
    {
        await AppService.UpdatePermissionsAsync(id, input);
    }
}