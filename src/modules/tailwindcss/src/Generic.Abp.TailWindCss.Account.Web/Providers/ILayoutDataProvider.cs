using JetBrains.Annotations;

namespace Generic.Abp.TailWindCss.Account.Web.Providers;

public interface ILayoutDataProvider
{
    string AppName { get; }
    string Title { get; }
    [CanBeNull] string Description { get; }
    [CanBeNull] string FaviconType { get; }
    [CanBeNull] string FaviconUrl { get; }
    [CanBeNull] string MobileNavIconClass { get; } //fa-solid fa-bars
    [CanBeNull] string LogoUrl { get; }
    [CanBeNull] string LogoClass { get; }
    [CanBeNull] string AccountHeroBackgroundUrl { get; }
}