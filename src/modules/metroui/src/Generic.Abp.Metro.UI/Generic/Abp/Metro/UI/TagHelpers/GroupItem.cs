namespace Generic.Abp.Metro.UI.TagHelpers;

public class GroupItem : IGroupItem
{
    public string Name { get; set; }
    public string HtmlContent { get; set; }
    public int DisplayOrder { get; set; }
}