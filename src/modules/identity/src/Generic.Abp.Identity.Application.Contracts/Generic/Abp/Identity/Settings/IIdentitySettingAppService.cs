using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Generic.Abp.Identity.Settings;

public interface IIdentitySettingAppService : IApplicationService
{
    Task<IdentitySettingDto> GetAsync();
    Task UpdateAsync(IdentitySettingDto input);
}