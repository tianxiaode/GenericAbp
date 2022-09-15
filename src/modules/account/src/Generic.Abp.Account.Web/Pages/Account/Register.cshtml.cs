using Generic.Abp.Account.Settings;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Validation;

namespace Generic.Abp.Account.Web.Pages.Account
{
    public class RegisterModel : AccountPageModel
    {
        protected IAccountAppService AccountAppService { get; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        //[BindProperty]
        public RegisterInfoModel RegisterInfoModel { get; set; }

        public RegisterModel(IAccountAppService accountAppService)
        {
            AccountAppService = accountAppService;
        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            await CheckSelfRegistrationAsync();

            return Page();
        }

        public virtual Task<IActionResult> OnPostAsync()
        {
            return Task.FromResult<IActionResult>(Page());
        }

        //public virtual async Task<IActionResult> OnPostAsync()
        //{
        //    ValidateModel();

        //    await CheckSelfRegistrationAsync();

        //    var registerDto = new RegisterDto
        //    {
        //        AppName = "MVC",
        //        EmailAddress = Input.EmailAddress,
        //        Password = Input.Password,
        //        UserName = Input.UserName,
        //        PhoneNumber = Input.PhoneNumber
        //    };
        //    try
        //    {
        //        var userDto = await AccountAppService.RegisterAsync(registerDto);
        //        var user = await UserManager.GetByIdAsync(userDto.Id);

        //        await UserManager.SetEmailAsync(user, Input.EmailAddress);
        //        await UserManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
        //        await SignInManager.SignInAsync(user, isPersistent: false);

        //    }
        //    catch (Exception e)
        //    {
        //        Logger.LogInformation(e.Message, e);
        //        return NoContent();
        //    }

        //    return Redirect(ReturnUrl ?? "~/"); //TODO: How to ensure safety? IdentityServer requires it however it should be checked somehow!
        //}

        protected virtual async Task CheckSelfRegistrationAsync()
        {
            if (!await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled) ||
                !await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
            {
                throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
            }
        }

    }

    public class RegisterInfoModel
    {
        public string AppName { get; set; } = "MVC";

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string EmailAddress { get; set; }

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [DataType(DataType.Password)]
        [DisableAuditing]
        public string Password { get; set; }

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [DisableAuditing]
        public string PasswordConfirm { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
        public string PhoneNumber { get; set; }

    }

}
