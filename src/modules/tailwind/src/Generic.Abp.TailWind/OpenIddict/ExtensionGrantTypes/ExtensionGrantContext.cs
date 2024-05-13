using OpenIddict.Abstractions;

namespace Generic.Abp.Tailwind.OpenIddict.ExtensionGrantTypes;

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