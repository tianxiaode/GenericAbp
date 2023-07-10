using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace Generic.Abp.Identity.Users;

[RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
[Area(IdentityRemoteServiceConsts.ModuleName)]
[ControllerName("Users")]
[Route("api/users")]
public class UserController : IdentityController, IUserAppService
{
    public UserController(IUserAppService userAppService)
    {
        UserAppService = userAppService;
    }

    private IUserAppService UserAppService { get; }

    [HttpGet]
    [Route("{id:guid}")]
    public Task<IdentityUserDto> GetAsync(Guid id)
    {
        return UserAppService.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
    {
        return UserAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id:guid}/roles")]
    public Task<PagedResultDto<UserGetRoleDto>> GetRolesAsync(Guid id, UserGetRolesInput input)
    {
        return UserAppService.GetRolesAsync(id, input);
    }


    [HttpPost]
    public Task<IdentityUserDto> CreateAsync([FromBody] UserCreateDto input)
    {
        return UserAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public Task<IdentityUserDto> UpdateAsync(Guid id, [FromBody] UserUpdateDto input)
    {
        return UserAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public Task<ListResultDto<IdentityUserDto>> DeleteAsync([FromBody] List<Guid> ids)
    {
        return UserAppService.DeleteAsync(ids);
    }

    [HttpPatch]
    [Route("{id:guid}/role/{roleId:guid}")]
    public Task AddRoleAsync(Guid id, Guid roleId)
    {
        return UserAppService.AddRoleAsync(id, roleId);
    }

    [HttpDelete]
    [Route("{id:guid}/role/{roleId:guid}")]
    public Task RemoveRoleAsync(Guid id, Guid roleId)
    {
        return UserAppService.RemoveRoleAsync(id, roleId);
    }

    [HttpPatch]
    [Route("{id:guid}/active/{value:bool}")]
    public Task SetActiveAsync(Guid id, bool value)
    {
        return UserAppService.SetActiveAsync(id, value);
    }

    [HttpPatch]
    [Route("{id:guid}/lockable/{value:bool}")]
    public Task SetLockoutEnabledAsync(Guid id, bool value)
    {
        return UserAppService.SetLockoutEnabledAsync(id, value);
    }

    [HttpPatch]
    [Route("{id:guid}/lockout/{value:bool}")]
    public Task<DateTimeOffset?> SetLockoutAsync(Guid id, bool value)
    {
        return UserAppService.SetLockoutAsync(id, value);
    }

    [HttpGet]
    [Route("by-username/{userName}")]
    public Task<IdentityUserDto> FindByUsernameAsync(string username)
    {
        return UserAppService.FindByUsernameAsync(username);
    }

    [HttpGet]
    [Route("by-email/{email}")]
    public Task<IdentityUserDto> FindByEmailAsync(string email)
    {
        return UserAppService.FindByEmailAsync(email);
    }

    [HttpPatch]
    [Route("{id:guid}/name")]
    public Task UpdateNameAsync(Guid id, [FromBody] UserUpdateNameDto input)
    {
        return UserAppService.UpdateNameAsync(id, input);
    }

    [HttpPatch]
    [Route("{id:guid}/surname")]
    public Task UpdateSurnameAsync(Guid id, [FromBody] UserUpdateSurnameDto input)
    {
        return UserAppService.UpdateSurnameAsync(id, input);
    }

    [HttpPatch]
    [Route("{id:guid}/email")]
    public Task UpdateEmailAsync(Guid id, [FromBody] UserUpdateEmailDto input)
    {
        return UserAppService.UpdateEmailAsync(id, input);
    }

    [HttpPatch]
    [Route("{id:guid}/phone-number")]
    public Task UpdatePhoneNumberAsync(Guid id, [FromBody] UserUpdatePhoneNumberDto input)
    {
        return UserAppService.UpdatePhoneNumberAsync(id, input);
    }
}