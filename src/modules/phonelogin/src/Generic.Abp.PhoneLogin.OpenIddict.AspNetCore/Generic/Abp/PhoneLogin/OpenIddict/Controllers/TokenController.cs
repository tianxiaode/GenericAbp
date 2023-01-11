using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Validation;

namespace Generic.Abp.PhoneLogin.OpenIddict.Controllers;

[Route("connect/token")]
[IgnoreAntiforgeryToken]
[ApiExplorerSettings(IgnoreApi = true)]
[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(Volo.Abp.OpenIddict.Controllers.TokenController), IncludeSelf = true)]
public class TokenController : Volo.Abp.OpenIddict.Controllers.TokenController
{
    protected PhoneLoginUserManager PhoneLoginUserManager => LazyServiceProvider.LazyGetRequiredService<PhoneLoginUserManager>();

    protected override async Task ReplaceEmailToUsernameOfInputIfNeeds(OpenIddictRequest request)
    {

        var userByUsername = await UserManager.FindByNameAsync(request.Username);
        if (userByUsername != null)
        {
            return;
        }

        var userByPhone = await PhoneLoginUserManager.FindByPhoneAsync(request.Username);
        if (userByPhone != null)
        {
            request.Username = userByPhone.UserName;
            return;
        }

        if (!ValidationHelper.IsValidEmailAddress(request.Username))
        {
            return;
        }

        var userByEmail = await UserManager.FindByEmailAsync(request.Username);
        if (userByEmail != null)
        {
            request.Username = userByEmail.UserName;
            return;
        }


        return;

    }

}
