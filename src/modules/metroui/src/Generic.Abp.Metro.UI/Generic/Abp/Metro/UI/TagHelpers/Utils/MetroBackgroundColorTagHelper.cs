using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers.Core;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Utils;

[HtmlTargetElement(Attributes = "background-color")]
public class MetroBackgroundColorTagHelper : MetroColorTagHelper
{
    [HtmlAttributeName("background-color")]
    public MetroColor Color { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await AddColorClassAsync(output, Color, true);
    }
}