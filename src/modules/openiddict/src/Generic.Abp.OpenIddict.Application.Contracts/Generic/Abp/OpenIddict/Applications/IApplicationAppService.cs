using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Generic.Abp.OpenIddict.Applications
{
    public interface IApplicationAppService : IApplicationService
    {
        Task<ApplicationDto> GetAsync(Guid id);
        Task<PagedResultDto<ApplicationDto>> GetListAsync(ApplicationGetListInput input);
        Task<ApplicationDto> CreateAsync(ApplicationCreateInput input);
        Task<ApplicationDto> UpdateAsync(Guid id, ApplicationUpdateInput input);
        Task DeleteAsync(Guid id);
    }
}