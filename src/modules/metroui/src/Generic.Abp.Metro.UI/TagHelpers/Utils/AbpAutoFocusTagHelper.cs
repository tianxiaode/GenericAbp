using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Utils;

[HtmlTargetElement(Attributes = "abp-auto-focus")]
public class AbpAutoFocusTagHelper : AbpTagHelper
{
    [HtmlAttributeName("abp-auto-focus")]
    public bool AutoFocus { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (AutoFocus)
        {
            output.Attributes.Add("data-auto-focus", "true");
        }
    }
}
