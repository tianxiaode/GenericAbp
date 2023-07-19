using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace Generic.Abp.Identity.Roles;

public interface IRoleAppService : IApplicationService
{
    Task<RoleDto> GetAsync(Guid id);
    Task<ListResultDto<RoleDto>> GetAllListAsync();
    Task<PagedResultDto<RoleDto>> GetListAsync(GetIdentityRolesInput input);
    Task<RoleDto> CreateAsync(RoleCreateDto input);
    Task<RoleDto> UpdateAsync(Guid id, RoleUpdateDto input);
    Task<ListResultDto<RoleDto>> DeleteAsync(List<Guid> ids);
    Task SetDefaultAsync(Guid id, bool value);
    Task SetPublicAsync(Guid id, bool value);
    Task<ListResultDto<RoleTranslationDto>> GetTranslationAsync(Guid id);
    Task UpdateTranslationAsync(Guid id, RoleTranslationDto[] translations);
}