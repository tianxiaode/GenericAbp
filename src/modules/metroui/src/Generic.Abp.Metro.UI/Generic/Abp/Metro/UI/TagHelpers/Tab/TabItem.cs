namespace Generic.Abp.Metro.UI.TagHelpers.Tab;

public class TabItem : ITabItem
{
    public string Title { get; set; }
    public string Target { get; set; }
    public int DisplayOrder { get; set; }
    public bool Active { get; set; } = false;
}