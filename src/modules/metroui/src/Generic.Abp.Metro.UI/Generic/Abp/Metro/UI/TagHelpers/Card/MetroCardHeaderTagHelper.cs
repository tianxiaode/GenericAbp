using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Card;

[HtmlTargetElement("metro-card-header", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroCardHeaderTagHelper : MetroCardTagHelperBase
{
    public string Text { get; set; }
    public string Image { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        await AddHeaderAsync(output, Text, Image);
    }
}