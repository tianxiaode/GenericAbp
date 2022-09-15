using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Generic.Abp.Identity.Users;

public interface IPasswordPolicyAppService : IApplicationService
{
    Task<PasswordPolicyDto> GetAsync();
    Task UpdateAsync(PasswordPolicyUpdateDto input);
}