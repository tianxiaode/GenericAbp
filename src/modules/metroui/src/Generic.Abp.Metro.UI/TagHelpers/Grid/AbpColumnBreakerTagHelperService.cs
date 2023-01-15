using Generic.Abp.Metro.UI.Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Grid;

public class AbpColumnBreakerTagHelperService : AbpTagHelperService<AbpColumnBreakerTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.AddClass("w-100");
        output.TagMode = TagMode.StartTagAndEndTag;
    }
}
