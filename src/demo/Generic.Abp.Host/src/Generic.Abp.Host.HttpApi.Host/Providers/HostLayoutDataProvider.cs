using Generic.Abp.TailWindCss.Account.Web.Providers;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Host.Providers;

[Dependency(ReplaceServices = true)]
public class HostLayoutDataProvider : DefaultLayoutDataProvider
{
    public override string AppName { get; set; } = "Generic Abp Host Demo";
    public override string Title { get; set; } = "Generic Abp Host Demo";
    public override string? Description { get; set; } = "Test";
    public override string? FaviconType { get; set; } = "image/png";
    public override string? FaviconUrl { get; set; } = "/images/logo.png";
    public override string? MobileNavIconClass { get; set; } = "fa-solid fa-bars";
    public override string? LogoUrl { get; set; } = "/images/logo.png";
    public override string? AccountHeroBackgroundUrl { get; set; } = "/images/bg.jpg";
}