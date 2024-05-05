using Microsoft.AspNetCore.Http;
using OpenIddict.Abstractions;

namespace Generic.Abp.TailWindCss.Account.Web.OpenIddict.ExtensionGrantTypes;

public class ExtensionGrantContext
{
    public HttpContext HttpContext { get; }

    public OpenIddictRequest Request { get; }

    public ExtensionGrantContext(HttpContext httpContext, OpenIddictRequest request)
    {
        HttpContext = httpContext;
        Request = request;
    }
}