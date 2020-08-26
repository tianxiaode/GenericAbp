using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace Generic.Abp.Account
{
    [RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
    [Area("account")]
    [Route("api/account")]
    public class AccountController : AbpController, IAccountAppService
    {
        protected IAccountAppService AccountAppService { get; }

        public AccountController(IAccountAppService accountAppService)
        {
            AccountAppService = accountAppService;
        }

        [HttpPost]
        [Route("register")]
        public virtual Task<IdentityUserDto> RegisterAsync(RegisterDto input)
        {
            return AccountAppService.RegisterAsync(input);
        }

        [HttpPost]
        [Route("send-verification-code")]
        public virtual Task<SendVerificationCodeResult> SendVerificationCodeAsync(SendVerificationCodeDto input)
        {
            return AccountAppService.SendVerificationCodeAsync(input);
        }

        [HttpPost]
        [Route("check-verification-code")]
        public virtual Task<CheckVerificationCodeResultDto> CheckVerificationCodeAsync(
            CheckVerificationCodeInputDto input)
        {
            return AccountAppService.CheckVerificationCodeAsync(input);
        }

        [HttpPost]
        [Route("reset-password")]
        public virtual Task ResetPasswordAsync(ResetPasswordInputDto input)
        {
            return AccountAppService.ResetPasswordAsync(input);
        }
    }
}
