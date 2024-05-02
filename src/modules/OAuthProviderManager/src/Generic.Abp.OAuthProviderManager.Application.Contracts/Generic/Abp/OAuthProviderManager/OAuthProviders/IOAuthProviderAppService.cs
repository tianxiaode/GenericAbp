using Generic.Abp.OAuthProviderManager.OAuthProviders.Dtos;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Generic.Abp.OAuthProviderManager.OAuthProviders;

public interface IOAuthProviderAppService : IApplicationService
{
    Task<ListResultDto<OAuthProviderDto>> GetListAsync(OAuthProviderGetListInput input);
    Task<OAuthProviderDto> UpdateAsync(string provider, OAuthProviderUpdateDto input);
}