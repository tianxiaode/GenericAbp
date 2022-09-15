using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace Generic.Abp.Account
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IdentityUserDto> RegisterAsync(RegisterDto input);

        Task<SendVerificationCodeResult> SendVerificationCodeAsync(SendVerificationCodeDto input);

        Task<CheckVerificationCodeResultDto> CheckVerificationCodeAsync(CheckVerificationCodeInputDto input);

        Task ResetPasswordAsync(ResetPasswordInputDto input);
    }
}
