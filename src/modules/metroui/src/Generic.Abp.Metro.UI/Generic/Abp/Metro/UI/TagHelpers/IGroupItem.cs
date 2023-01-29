namespace Generic.Abp.Metro.UI.TagHelpers;

public interface IGroupItem
{
    public string Name { get; set; }
    string HtmlContent { get; set; }
    int DisplayOrder { get; set; }
}