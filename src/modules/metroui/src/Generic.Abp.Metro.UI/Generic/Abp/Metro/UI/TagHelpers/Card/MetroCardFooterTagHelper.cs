using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Card;

[HtmlTargetElement("metro-card-footer", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroCardFooterTagHelper : MetroCardTagHelperBase
{
    public MetroCardFooterTagHelper(HtmlEncoder htmlEncoder) : base(htmlEncoder)
    {
    }

    public string Text { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        var child = await output.GetChildContentAsync();
        await AddFooterAsync(output, Text ?? child.GetContent());
        await AddGroupItemAsync(context, nameof(MetroCardFooterTagHelper), await output.RenderAsync(HtmlEncoder));
        output.SuppressOutput();
    }
}