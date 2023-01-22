using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Utils;


[HtmlTargetElement(Attributes = "metro-class")]
public class MetroClassTagHelper: MetroTagHelper
{
    [HtmlAttributeName("metro-class")]
    public string Class { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.AddClass(Class);
        return Task.CompletedTask;
    }
}