using Microsoft.AspNetCore.Authentication;
using OpenIddict.Validation.AspNetCore;

namespace Generic.Abp.Tailwind.Microsoft.AspNetCore.Builder;

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
                if (result is { Succeeded: true, Principal: not null })
                {
                    ctx.User = result.Principal;
                }
            }

            await next();
        });
    }
}