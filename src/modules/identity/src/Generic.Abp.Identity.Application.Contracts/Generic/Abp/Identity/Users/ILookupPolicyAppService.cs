using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Generic.Abp.Identity.Users;

public interface ILookupPolicyAppService: IApplicationService
{
    Task<LookupPolicyDto> GetAsync();
    Task UpdateAsync(LookupPolicyUpdateDto input);

}