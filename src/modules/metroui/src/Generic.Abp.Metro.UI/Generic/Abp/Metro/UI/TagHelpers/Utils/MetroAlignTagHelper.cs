using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Utils;

[HtmlTargetElement(Attributes = "align")]
public class MetroAlignTagHelper : TagHelper
{
    [HtmlAttributeName("align")] public Alignment Alignment { get; set; } = Alignment.Left;

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (Alignment == Alignment.Left) return Task.CompletedTask;

        output.Attributes.AddClass("text-" + Alignment.ToString()?.ToLowerInvariant());

        return Task.CompletedTask;
    }
}