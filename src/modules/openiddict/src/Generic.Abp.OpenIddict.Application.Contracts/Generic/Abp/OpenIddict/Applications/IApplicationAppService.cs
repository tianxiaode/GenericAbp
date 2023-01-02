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

    }
}
