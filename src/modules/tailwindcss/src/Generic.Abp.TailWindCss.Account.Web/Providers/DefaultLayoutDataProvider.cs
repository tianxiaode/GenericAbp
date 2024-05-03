using Volo.Abp.DependencyInjection;

namespace Generic.Abp.TailWindCss.Account.Web.Providers;

public class DefaultLayoutDataProvider : ILayoutDataProvider, ITransientDependency
{
    public virtual string AppName { get; set; } = "App";
    public virtual string Title { get; set; } = "App";
    public virtual string Description { get; set; }
    public virtual string FaviconType { get; set; }
    public virtual string FaviconUrl { get; set; }
    public virtual string MobileNavIconClass { get; set; } = "fa-solid fa-bars";
    public virtual string LogoUrl { get; set; }
    public virtual string LogoClass { get; set; }
    public virtual string AccountHeroBackgroundUrl { get; set; }
}