using Generic.Abp.Account.Localization;
using Generic.Abp.Account.Settings;
using Generic.Abp.TextTemplate.PasswordReset;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Emailing;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Generic.Abp.Account
{
    public class AccountAppService : ApplicationService, IAccountAppService
    {
        protected IIdentityRoleRepository RoleRepository { get; }
        protected IdentityUserManager UserManager { get; }
        protected AbpLocalizationOptions LocalizationOptions { get; }

        protected PasswordResetTemplateService PasswordResetTemplateService { get; }

        protected IDistributedCache<VerificationCodeCacheItem, Guid> CodeCache { get; }

        protected IEmailSender EmailSender { get; }

        public AccountAppService(
            IOptions<AbpLocalizationOptions> localizationOptions,
            IdentityUserManager userManager,
            IIdentityRoleRepository roleRepository,
            PasswordResetTemplateService passwordResetTemplateService,
            IDistributedCache<VerificationCodeCacheItem, Guid> codeCache, IEmailSender emailSender)
        {
            LocalizationResource = typeof(AccountResource);
            LocalizationOptions = localizationOptions.Value;
            RoleRepository = roleRepository;
            PasswordResetTemplateService = passwordResetTemplateService;
            CodeCache = codeCache;
            EmailSender = emailSender;
            UserManager = userManager;
        }

        public virtual async Task<IdentityUserDto> RegisterAsync(RegisterDto input)
        {
            await CheckSelfRegistrationAsync();

            var user = new IdentityUser(GuidGenerator.Create(), input.UserName, input.EmailAddress, CurrentTenant.Id);


            (await UserManager.CreateAsync(user, input.Password)).CheckErrors();

            await UserManager.SetEmailAsync(user, input.EmailAddress);
            await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber);
            await UserManager.AddDefaultRolesAsync(user);

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        protected virtual async Task CheckSelfRegistrationAsync()
        {
            if (!await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled))
            {
                throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
            }
        }


        public virtual async Task<SendVerificationCodeResult> SendVerificationCodeAsync(SendVerificationCodeDto input)
        {
            var user = await UserManager.FindByEmailAsync(input.EmailAddress);
            if (user == null) return new SendVerificationCodeResult(SendVerificationCodeResultType.InvalidEmail);
            var localizer = ServiceProvider.GetRequiredService(
                typeof(IStringLocalizer<>).MakeGenericType(LocalizationOptions.DefaultResourceType)
            ) as IStringLocalizer;

            var companyName = localizer == null ? "" : localizer["AppName"];
            var userName = user.Name ?? user.UserName;
            var rnd = new Random((int)DateTime.Now.Ticks);
            var code = rnd.Next(100000, 999999);

            var cacheItem = await CodeCache.GetOrAddAsync(user.Id,
                () => Task.FromResult(new VerificationCodeCacheItem(0, code.ToString())),
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddHours(1)
                });
            if (cacheItem.Count >= 5)
            {
                cacheItem.Code = "";
                await CodeCache.SetAsync(user.Id, cacheItem, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddHours(24)
                });
                return new SendVerificationCodeResult(SendVerificationCodeResultType.MaxCount);
            }

            cacheItem.Code = code.ToString();
            cacheItem.Count += 1;
            await CodeCache.SetAsync(user.Id, cacheItem, new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddHours(1)

            });
            var subject = string.Format(L["VerificationCodeMailSubject"], userName);
            var body =
                await PasswordResetTemplateService.RunAsync(new PasswordResetModel(companyName, userName,
                    code.ToString()));
            try
            {
                await EmailSender.SendAsync(user.Email, subject, body);
                return new SendVerificationCodeResult(SendVerificationCodeResultType.Success);

            }
            catch (Exception e)
            {
                Logger.LogDebug(e.Message, e);
                Logger.LogDebug(e.StackTrace, e);
                Logger.LogInformation($"Error mail message：{user.Email},{subject},{ body}");
                return new SendVerificationCodeResult(SendVerificationCodeResultType.Error);
            }

        }

        public virtual async Task<CheckVerificationCodeResultDto> CheckVerificationCodeAsync(CheckVerificationCodeInputDto input)
        {
            var user = await UserManager.FindByEmailAsync(input.EmailAddress);
            if (user == null) throw new UserFriendlyException(L["InvalidEmail"]);
            var cacheItem = await CodeCache.GetAsync(user.Id);
            if (cacheItem == null) throw new UserFriendlyException(L["InvalidVerification"]);
            if (cacheItem.Code != input.Code) throw new UserFriendlyException(L["InvalidVerification"]);
            await CodeCache.RemoveAsync(user.Id);
            return new CheckVerificationCodeResultDto(input.EmailAddress, await UserManager.GeneratePasswordResetTokenAsync(user));
        }

        public virtual async Task ResetPasswordAsync(ResetPasswordInputDto input)
        {
            var user = await UserManager.FindByEmailAsync(input.EmailAddress);
            if (user == null) throw new UserFriendlyException(L["InvalidEmail"]);
            await UserManager.ResetPasswordAsync(user, input.Token, input.NewPassword);
        }

    }
}
