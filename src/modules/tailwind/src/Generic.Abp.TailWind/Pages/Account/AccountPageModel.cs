using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Generic.Abp.Tailwind.Pages.Account;

public abstract class AccountPageModel : AbpPageModel
{
    protected readonly IAccountAppService AccountAppService;
    protected readonly SignInManager<IdentityUser> SignInManager;
    protected readonly IdentityUserManager UserManager;
    protected readonly IdentitySecurityLogManager IdentitySecurityLogManager;
    protected readonly IOptions<IdentityOptions> IdentityOptions;
    protected readonly IExceptionToErrorInfoConverter ExceptionToErrorInfoConverter;
    public string? Error { get; set; }

    protected AccountPageModel(IAccountAppService accountAppService, SignInManager<IdentityUser> signInManager,
        IdentityUserManager userManager, IdentitySecurityLogManager identitySecurityLogManager,
        IOptions<IdentityOptions> identityOptions, IExceptionToErrorInfoConverter exceptionToErrorInfoConverter)
    {
        AccountAppService = accountAppService;
        SignInManager = signInManager;
        UserManager = userManager;
        IdentitySecurityLogManager = identitySecurityLogManager;
        IdentityOptions = identityOptions;
        ExceptionToErrorInfoConverter = exceptionToErrorInfoConverter;
        LocalizationResourceType = typeof(AccountResource);
        ObjectMapperContext = typeof(GenericAbpTailwindModule);
    }

    protected virtual void CheckCurrentTenant(Guid? tenantId)
    {
        if (CurrentTenant.Id != tenantId)
        {
            throw new ApplicationException(
                $"Current tenant is different than given tenant. CurrentTenant.Id: {CurrentTenant.Id}, given tenantId: {tenantId}");
        }
    }

    protected virtual void CheckIdentityErrors(IdentityResult identityResult)
    {
        if (!identityResult.Succeeded)
        {
            throw new UserFriendlyException("Operation failed: " +
                                            identityResult.Errors.Select(e => $"[{e.Code}] {e.Description}")
                                                .JoinAsString(", "));
        }

        //identityResult.CheckErrors(LocalizationManager); //TODO: Get from old Abp
    }

    protected virtual string? GetLocalizeExceptionMessage(Exception exception)
    {
        if (exception is ILocalizeErrorMessage || exception is IHasErrorCode)
        {
            return ExceptionToErrorInfoConverter.Convert(exception, false).Message;
        }

        return exception.Message;
    }

    protected virtual async Task<string> GetUserNameFromEmail(string email)
    {
        var userName = email.Split('@')[0];
        var existUser = await UserManager.FindByNameAsync(userName);
        while (existUser != null)
        {
            var randomUserName = userName + RandomHelper.GetRandom(1000, 9999);
            existUser = await UserManager.FindByNameAsync(randomUserName);
            if (existUser == null)
            {
                userName = randomUserName;
                break;
            }
        }

        return userName;
    }
}