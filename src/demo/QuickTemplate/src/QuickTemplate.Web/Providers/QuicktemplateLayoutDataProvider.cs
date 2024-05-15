#nullable enable
using Generic.Abp.Tailwind.Providers;
using Volo.Abp.DependencyInjection;

namespace QuickTemplate.Web.Providers;

[Dependency(ReplaceServices = true)]
public class QuicktemplateLayoutDataProvider : DefaultLayoutDataProvider
{
    public override string AppName { get; set; } = "Quick Template";
    public override string Title { get; set; } = "Quick Template";
    public override string? Description { get; set; } = "Test";
    public override string? FaviconType { get; set; } = "image/png";
    public override string? FaviconUrl { get; set; } = "/images/logo.png";
    public override string? MobileNavIconClass { get; set; } = "fa-solid fa-bars";
    public override string? LogoUrl { get; set; } = "/images/logo.png";
    public override string? AccountHeroBackgroundUrl { get; set; } = "/images/bg.jpg";
}