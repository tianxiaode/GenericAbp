namespace Generic.Abp.Tailwind;

public static class TailwindApplicationBuilderErrorPageExtensions
{
    public static IApplicationBuilder UseErrorPage(this IApplicationBuilder app)
    {
        return app
            .UseStatusCodePagesWithRedirects("~/Error?httpStatusCode={0}")
            .UseExceptionHandler("/Error");
    }
}