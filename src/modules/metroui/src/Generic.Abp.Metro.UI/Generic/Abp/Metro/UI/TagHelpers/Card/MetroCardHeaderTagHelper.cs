using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Card;

[HtmlTargetElement("metro-card-header", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroCardHeaderTagHelper : MetroCardTagHelperBase
{
    public MetroCardHeaderTagHelper(HtmlEncoder htmlEncoder) : base(htmlEncoder)
    {
    }

    public string Text { get; set; }
    public string Image { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        var child = await output.GetChildContentAsync();
        var html = await AddHeaderAsync(output, Text ?? child.GetContent(), Image);
        await AddGroupItemAsync(context, nameof(MetroCardHeaderTagHelper), html);
        output.SuppressOutput();
    }
}