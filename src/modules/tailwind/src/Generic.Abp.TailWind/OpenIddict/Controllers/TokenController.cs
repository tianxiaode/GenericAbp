using Generic.Abp.Tailwind.OpenIddict.ExtensionGrantTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Volo.Abp;

namespace Generic.Abp.Tailwind.OpenIddict.Controllers;

[Route("connect/token")]
[IgnoreAntiforgeryToken]
[ApiExplorerSettings(IgnoreApi = true)]
public partial class TokenController : AbpOpenIdDictControllerBase
{
    [HttpGet, HttpPost, Produces("application/json")]
    public virtual async Task<IActionResult> HandleAsync()
    {
        var request = await GetOpenIddictServerRequestAsync(HttpContext);

        if (request.IsPasswordGrantType())
        {
            return await HandlePasswordAsync(request);
        }

        if (request.IsAuthorizationCodeGrantType())
        {
            return await HandleAuthorizationCodeAsync(request);
        }

        if (request.IsRefreshTokenGrantType())
        {
            return await HandleRefreshTokenAsync(request);
        }

        if (request.IsDeviceCodeGrantType())
        {
            return await HandleDeviceCodeAsync(request);
        }

        if (request.IsClientCredentialsGrantType())
        {
            return await HandleClientCredentialsAsync(request);
        }

        if (request.GrantType.IsNullOrEmpty())
        {
            throw new AbpException(string.Format(L["TheSpecifiedGrantTypeIsNotImplemented"], request.GrantType));
        }

        var extensionGrantsOptions =
            HttpContext.RequestServices.GetRequiredService<IOptions<AbpOpenIddictExtensionGrantsOptions>>();

        if (extensionGrantsOptions.Value.Grants.All(m => m.Key != request.GrantType))
        {
            throw new AbpException(string.Format(L["TheSpecifiedGrantTypeIsNotImplemented"], request.GrantType));
        }

        var extensionTokenGrant = extensionGrantsOptions.Value.Find<ITokenExtensionGrant>(request.GrantType);
        return await extensionTokenGrant.HandleAsync(new ExtensionGrantContext(HttpContext, request));
    }
}