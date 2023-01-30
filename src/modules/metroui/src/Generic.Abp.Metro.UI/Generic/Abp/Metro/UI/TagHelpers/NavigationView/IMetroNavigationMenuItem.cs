namespace Generic.Abp.Metro.UI.TagHelpers.NavigationView;

public interface IMetroNavigationMenuItem
{
    string Text { get; }
    MetroNavigationMenuItemType Type { get; }
    string Icon { get; }
    string Url { get; }
    string Value { get; }
}