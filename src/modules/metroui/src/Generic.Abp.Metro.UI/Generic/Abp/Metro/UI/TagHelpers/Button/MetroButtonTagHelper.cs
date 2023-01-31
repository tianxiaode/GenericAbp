using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

[HtmlTargetElement("metro-button", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroButtonTagHelper : ButtonTagHelperBase
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "button";
        await base.ProcessAsync(context, output);
        output.Content.AppendHtml(await output.GetChildContentAsync());
    }
}