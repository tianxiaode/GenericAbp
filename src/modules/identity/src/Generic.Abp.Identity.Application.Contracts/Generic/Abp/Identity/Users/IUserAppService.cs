using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace Generic.Abp.Identity.Users;

public interface IUserAppService : IApplicationService
{
    Task<IdentityUserDto> GetAsync(Guid id);
    Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input);
    Task<PagedResultDto<UserGetRoleDto>> GetRolesAsync(Guid id, UserGetRolesInput input);
    Task<IdentityUserDto> CreateAsync(UserCreateDto input);
    Task<IdentityUserDto> UpdateAsync(Guid id, UserUpdateDto input);
    Task<ListResultDto<IdentityUserDto>> DeleteAsync(List<Guid> ids);
    Task AddRoleAsync(Guid id, Guid roleId);
    Task RemoveRoleAsync(Guid id, Guid roleId);
    Task SetActiveAsync(Guid id, bool value);
    Task SetLockoutEnabledAsync(Guid id, bool value);
    Task<DateTimeOffset?> SetLockoutAsync(Guid id, bool value);
    Task<IdentityUserDto> FindByUsernameAsync(string username);
    Task<IdentityUserDto> FindByEmailAsync(string email);
    Task UpdateNameAsync(Guid id, UserUpdateNameDto input);
    Task UpdateSurnameAsync(Guid id, UserUpdateSurnameDto input);
    Task UpdateEmailAsync(Guid id, UserUpdateEmailDto input);
    Task UpdatePhoneNumberAsync(Guid id, UserUpdatePhoneNumberDto input);
}