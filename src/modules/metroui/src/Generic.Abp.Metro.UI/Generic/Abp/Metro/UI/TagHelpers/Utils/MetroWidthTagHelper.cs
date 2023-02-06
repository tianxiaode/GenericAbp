using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Utils;

[HtmlTargetElement(Attributes = "width")]
public class MetroWidthTagHelper : TagHelper
{
    [HtmlAttributeName("width")] public string Width { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrWhiteSpace(Width)) return Task.CompletedTask;
        output.Attributes.AddSizeStyle(Width);
        return Task.CompletedTask;
    }
}