using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Utils;

[HtmlTargetElement(Attributes = "metro-auto-focus")]
public class MetroAutoFocusTagHelper: MetroTagHelper
{
    [HtmlAttributeName("metro-auto-focus")]
    public bool AutoFocus { get; set; } = false;

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (AutoFocus)
        {
            output.Attributes.Add("autofocus","autofocus");

        }
        return Task.CompletedTask;
    }

}