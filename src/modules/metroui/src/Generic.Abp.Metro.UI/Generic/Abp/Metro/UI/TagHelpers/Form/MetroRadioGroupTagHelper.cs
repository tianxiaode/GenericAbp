using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Localization;
using Volo.Abp.Localization;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroRadioGroupTagHelper : MetroInputGroupTagHelper
{
    public MetroRadioGroupTagHelper(
        HtmlEncoder htmlEncoder,
        IHtmlGenerator generator,
        IMetroTagHelperLocalizerService localizer,
        SelectItemsService selectItemsService) : base(htmlEncoder, generator, localizer, selectItemsService)
    {
        IsRadioGroup = true;
    }
}