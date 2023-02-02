using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Card;

[HtmlTargetElement("metro-card-footer", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroCardFooterTagHelper : MetroCardTagHelperBase
{
    public string Text { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        await AddFooterAsync(output, Text);
    }
}