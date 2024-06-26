﻿using Microsoft.AspNetCore.Identity;
using Volo.Abp.OpenIddict;

namespace Generic.Abp.Tailwind.OpenIddict;

public static class AbpSignInResultExtensions
{
    public static string ToOpenIddictIdentitySecurityLogAction(this SignInResult result)
    {
        if (result.Succeeded)
        {
            return OpenIddictSecurityLogActionConsts.LoginSucceeded;
        }

        if (result.IsLockedOut)
        {
            return OpenIddictSecurityLogActionConsts.LoginLockedout;
        }

        if (result.RequiresTwoFactor)
        {
            return OpenIddictSecurityLogActionConsts.LoginRequiresTwoFactor;
        }

        if (result.IsNotAllowed)
        {
            return OpenIddictSecurityLogActionConsts.LoginNotAllowed;
        }

        if (!result.Succeeded)
        {
            return OpenIddictSecurityLogActionConsts.LoginFailed;
        }

        return OpenIddictSecurityLogActionConsts.LoginFailed;
    }
}