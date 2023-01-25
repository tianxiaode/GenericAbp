namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public interface IFormItem
{
    string HtmlContent { get; set; }
    int Order { get; set; }
    string Name { get; set; }
}