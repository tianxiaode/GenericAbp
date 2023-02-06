using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Utils;

[HtmlTargetElement(Attributes = "height")]
public class MetroHeightTagHelper : TagHelper
{
    [HtmlAttributeName("height")] public string Height { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrWhiteSpace(Height)) return Task.CompletedTask;
        output.Attributes.AddSizeStyle(Height, false);
        return Task.CompletedTask;
    }
}