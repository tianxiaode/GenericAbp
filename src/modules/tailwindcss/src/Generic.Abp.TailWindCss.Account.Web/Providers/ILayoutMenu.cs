using JetBrains.Annotations;

namespace Generic.Abp.TailWindCss.Account.Web.Providers;

public interface ILayoutMenu
{
    string Name { get; }
    string Href { get; }
    [CanBeNull] string[] Permissions { get; }
}