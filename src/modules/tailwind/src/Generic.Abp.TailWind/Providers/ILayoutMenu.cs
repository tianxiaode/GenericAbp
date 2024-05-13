using JetBrains.Annotations;

namespace Generic.Abp.Tailwind.Providers;

public interface ILayoutMenu
{
    string Name { get; }
    string Href { get; }
    string[]? Permissions { get; }
}