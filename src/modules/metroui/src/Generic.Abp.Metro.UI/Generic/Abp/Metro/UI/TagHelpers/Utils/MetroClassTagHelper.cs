using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Utils;


[HtmlTargetElement(Attributes = "metro-class")]
public class MetroClassTagHelper: MetroTagHelper
{
    [HtmlAttributeName("metro-class")]
    public string Class { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.AddClass(Class);
    }
}