namespace Generic.Abp.Metro.UI.TagHelpers.NavigationView;

public class MetroNavigationMenuItem : IMetroNavigationMenuItem
{
    public string Text { get; set; }
    public MetroNavigationMenuItemType Type { get; set; } = MetroNavigationMenuItemType.Default;
    public string Icon { get; set; }
    public string Url { get; set; }
    public string Value { get; set; }
    public int DisplayOrder { get; set; } = 0;
}