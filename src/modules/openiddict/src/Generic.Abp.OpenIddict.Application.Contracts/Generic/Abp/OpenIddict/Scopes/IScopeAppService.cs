using System.Threading.Tasks;
using System;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace Generic.Abp.OpenIddict.Scopes
{
    public interface IScopeAppService : IApplicationService
    {
        Task<ScopeDto> GetAsync(Guid id);
        Task<PagedResultDto<ScopeDto>> GetListAsync(ScopeGetListInput input);
        Task<ScopeDto> CreateAsync(ScopeCreateInput input);
        Task<ScopeDto> UpdateAsync(Guid id, ScopeUpdateInput input);
        Task<ListResultDto<ScopeDto>> DeleteAsync(List<Guid> ids);
        Task<List<string>> GetPropertiesAsync(Guid id);
        Task AddPropertyAsync(Guid id, ScopePropertyCreateInput input);
        Task RemovePropertyAsync(Guid id, ScopePropertyDeleteInput input);
        Task<List<string>> GetResourcesAsync(Guid id);
        Task AddResourceAsync(Guid id, ScopeResourceCreateInput input);
        Task RemoveResourceAsync(Guid id, ScopeResourceDeleteInput input);
    }
}