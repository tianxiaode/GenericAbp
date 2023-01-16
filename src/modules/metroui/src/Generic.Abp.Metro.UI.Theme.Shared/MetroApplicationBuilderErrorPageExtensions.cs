using Microsoft.AspNetCore.Builder;

namespace Generic.Abp.Metro.UI.Theme.Shared;

public static class MetroApplicationBuilderErrorPageExtensions
{
    public static IApplicationBuilder UseErrorPage(this IApplicationBuilder app)
    {
        return app
            .UseStatusCodePagesWithRedirects("~/Error?httpStatusCode={0}")
            .UseExceptionHandler("/Error");
    }
}
