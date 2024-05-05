﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using OpenIddict.Validation.AspNetCore;

namespace Generic.Abp.TailWindCss.Account.Web.Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderAbpOpenIddictMiddlewareExtension
{
    public static IApplicationBuilder UseAbpOpenIddictValidation(this IApplicationBuilder app,
        string schema = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
    {
        return app.Use(async (ctx, next) =>
        {
            if (ctx.User.Identity?.IsAuthenticated != true)
            {
                var result = await ctx.AuthenticateAsync(schema);
                if (result.Succeeded && result.Principal != null)
                {
                    ctx.User = result.Principal;
                }
            }

            await next();
        });
    }
}