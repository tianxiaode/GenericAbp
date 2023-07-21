using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Generic.Abp.Identity.Roles;

[RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
[Area(IdentityRemoteServiceConsts.ModuleName)]
[ControllerName("Roles")]
[Route("api/roles")]
public class RoleController : IdentityController, IRoleAppService
{
    private readonly IRoleAppService _roleAppService;

    public RoleController(IRoleAppService roleAppService)
    {
        _roleAppService = roleAppService;
    }

    [HttpGet]
    [Route("{id:guid}")]
    public virtual Task<RoleDto> GetAsync(Guid id)
    {
        return _roleAppService.GetAsync(id);
    }

    [HttpGet]
    [Route("all")]
    public virtual Task<ListResultDto<RoleDto>> GetAllListAsync()
    {
        return _roleAppService.GetAllListAsync();
    }

    [HttpGet]
    public virtual Task<PagedResultDto<RoleDto>> GetListAsync(GetIdentityRolesInput input)
    {
        return _roleAppService.GetListAsync(input);
    }

    [HttpPost]
    public virtual Task<RoleDto> CreateAsync([FromBody] RoleCreateDto input)
    {
        return _roleAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public virtual Task<RoleDto> UpdateAsync(Guid id, [FromBody] RoleUpdateDto input)
    {
        return _roleAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public Task<ListResultDto<RoleDto>> DeleteAsync([FromBody] List<Guid> ids)
    {
        return _roleAppService.DeleteAsync(ids);
    }

    //[NonAction]
    //public Task<ListResultDto<RoleDto>> DeleteAsync(List<Guid> ids)
    //{
    //    throw new NotImplementedException();
    //}

    [HttpPatch]
    [Route("{id}/default/{value:bool}")]
    public Task SetDefaultAsync(Guid id, bool value)
    {
        return _roleAppService.SetDefaultAsync(id, value);
    }

    [HttpPatch]
    [Route("{id}/public/{value:bool}")]
    //[NonAction]
    public Task SetPublicAsync(Guid id, bool value)
    {
        return _roleAppService.SetPublicAsync(id, value);
    }

    [HttpGet]
    [Route("{id:guid}/translations")]
    public Task<ListResultDto<RoleTranslationDto>> GetTranslationAsync(Guid id)
    {
        return _roleAppService.GetTranslationAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/translations")]
    public Task UpdateTranslationAsync(Guid id, [FromBody] List<RoleTranslationDto> translations)
    {
        return _roleAppService.UpdateTranslationAsync(id, translations);
    }
}