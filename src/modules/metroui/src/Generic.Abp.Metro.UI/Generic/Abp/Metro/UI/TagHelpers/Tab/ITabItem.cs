namespace Generic.Abp.Metro.UI.TagHelpers.Tab;

public interface ITabItem
{
    string Title { get; }
    string Target { get; }
    bool Active { get; }
}