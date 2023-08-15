using System.Threading.Tasks;
using System;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace Generic.Abp.OpenIddict.Applications
{
    public interface IApplicationAppService : IApplicationService
    {
        Task<ApplicationDto> GetAsync(Guid id);
        Task<PagedResultDto<ApplicationDto>> GetListAsync(ApplicationGetListInput input);
        Task<ApplicationDto> CreateAsync(ApplicationCreateInput input);
        Task<ApplicationDto> UpdateAsync(Guid id, ApplicationUpdateInput input);
        Task<ListResultDto<ApplicationDto>> DeleteAsync(List<Guid> ids);
        Task<Dictionary<string, Dictionary<string, string>>> GetAllPermisions();
        Task<List<string>> GetPermissionsAsync(Guid id);
        Task AddPermissionAsync(Guid id, PermissionCreateInput input);
        Task RemovePermissionAsync(Guid id, PermissionDeleteInput input);
        Task<List<string>> GetPostLogoutRedirectUrisAsync(Guid id);
        Task AddPostLogoutRedirectUriAsync(Guid id, PostLogoutRedirectUriCreateInput input);
        Task RemovePostLogoutRedirectUriAsync(Guid id, PostLogoutRedirectUriDeleteInput input);
        Task<List<string>> GetPropertiesAsync(Guid id);
        Task AddPropertyAsync(Guid id, PropertyCreateInput input);
        Task RemovePropertyAsync(Guid id, PropertyDeleteInput input);
        Task<List<string>> GetRedirectUrisAsync(Guid id);
        Task AddRedirectUriAsync(Guid id, RedirectUriCreateInput input);
        Task RemoveRedirectUriAsync(Guid id, RedirectUriDeleteInput input);
        Task<List<string>> GetRequirementsAsync(Guid id);
        Task AddRequirementAsync(Guid id, RequirementCreateInput input);
        Task RemoveRequirementAsync(Guid id, RequirementDeleteInput input);
    }
}