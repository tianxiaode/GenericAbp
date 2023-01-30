using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

[HtmlTargetElement("metro-toolbar-separator", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroToolbarSeparatorTagHelper : MetroTagHelper
{
    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        var attributes = output.Attributes;
        attributes.AddClass("border-1");
        attributes.AddClass("bd-gray");
        attributes.AddClass("border-solid");
        attributes.AddClass("mx-1");
        return Task.CompletedTask;
    }
}