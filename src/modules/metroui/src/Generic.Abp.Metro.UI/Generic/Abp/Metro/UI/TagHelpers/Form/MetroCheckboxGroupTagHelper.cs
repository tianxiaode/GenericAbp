using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Encodings.Web;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroCheckboxGroupTagHelper : MetroInputGroupTagHelper
{
    public MetroCheckboxGroupTagHelper(
        HtmlEncoder htmlEncoder,
        IHtmlGenerator generator,
        IMetroTagHelperLocalizerService localizer,
        SelectItemsService selectItemsService) : base(htmlEncoder, generator, localizer, selectItemsService)
    {
        IsCheckboxGroup = true;
    }
}