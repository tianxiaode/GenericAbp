namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class FormItem: ITagItem, IFormItem
{
    public string HtmlContent { get; set; }
    public int Order { get; set; }
    public string Name { get; set; }
}
