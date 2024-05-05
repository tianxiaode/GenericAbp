using Microsoft.AspNetCore.Http;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using Volo.Abp;

namespace Generic.Abp.TailWindCss.Account.Web.OpenIddict;

public static class AbpOpenIddictHttpContextExtensions
{
    public static OpenIddictServerTransaction GetOpenIddictServerTransaction(this HttpContext context)
    {
        Check.NotNull(context, nameof(context));
        return context.Features.Get<OpenIddictServerAspNetCoreFeature>()?.Transaction;
    }
}