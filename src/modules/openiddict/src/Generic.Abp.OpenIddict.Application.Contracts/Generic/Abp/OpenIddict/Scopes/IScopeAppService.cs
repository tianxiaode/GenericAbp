using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Generic.Abp.OpenIddict.Scopes
{
    public interface IScopeAppService : IApplicationService
    {
        Task<ScopeDto> GetAsync(Guid id);
        Task<PagedResultDto<ScopeDto>> GetListAsync(ScopeGetListInput input);
        Task<ListResultDto<ScopeDto>> GetAllAsync();
        Task<ScopeDto> CreateAsync(ScopeCreateInput input);
        Task<ScopeDto> UpdateAsync(Guid id, ScopeUpdateInput input);
        Task DeleteAsync(Guid id);
    }
}