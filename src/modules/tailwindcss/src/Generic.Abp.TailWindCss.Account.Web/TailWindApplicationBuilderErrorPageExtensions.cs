using Microsoft.AspNetCore.Builder;

namespace Generic.Abp.TailWindCss.Account.Web;

public static class TailWindApplicationBuilderErrorPageExtensions
{
    public static IApplicationBuilder UseErrorPage(this IApplicationBuilder app)
    {
        return app
            .UseStatusCodePagesWithRedirects("~/Error?httpStatusCode={0}")
            .UseExceptionHandler("/Error");
    }
}