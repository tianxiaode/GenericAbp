using Microsoft.AspNetCore.Builder;

namespace Generic.Abp.Themes.Shared
{
    public static class AbpApplicationBuilderErrorPageExtensions
    {
        public static IApplicationBuilder UseErrorPage(this IApplicationBuilder app)
        {
            return app
                .UseStatusCodePagesWithRedirects("~/Error?httpStatusCode={0}")
                .UseExceptionHandler("/Error");
        }
    }
}
