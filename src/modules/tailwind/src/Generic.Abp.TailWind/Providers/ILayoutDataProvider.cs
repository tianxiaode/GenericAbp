namespace Generic.Abp.Tailwind.Providers;

public interface ILayoutDataProvider
{
    string AppName { get; }
    string Title { get; }
    string? Description { get; }
    string? FaviconType { get; }
    string? FaviconUrl { get; }
    string? MobileNavIconClass { get; } //fa-solid fa-bars
    string? LogoUrl { get; }
    string? LogoClass { get; }
    string? AccountHeroBackgroundUrl { get; }
}