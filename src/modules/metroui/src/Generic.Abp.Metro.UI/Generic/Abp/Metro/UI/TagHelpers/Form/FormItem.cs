namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class FormItem: ITagItem
{
    public string HtmlContent { get; set; }
    public int Order { get; set; }
    public string Name { get; set; }
    public string PropertyName { get; set; }
}
