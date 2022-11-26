using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Generic.Abp.IdentityServer.ClaimTypes
{
    public interface IClaimTypeAppService: IApplicationService
    {
        Task<ListResultDto<ClaimTypeDto>> GetListAsync();
    }
}
