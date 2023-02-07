using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Utils;

[HtmlTargetElement(Attributes = "indent")]
public class IndentTagHelper : TagHelper
{
    [HtmlAttributeName("indent")] public bool Indent { get; set; } = false;

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var cultureName = CultureInfo.CurrentUICulture.Name;
        if (Indent && cultureName.StartsWith("zh"))
        {
            output.Attributes.AddClass("indent");
        }
        else
        {
            output.Attributes.AddClass("indent-letter");
        }

        return Task.CompletedTask;
    }
}