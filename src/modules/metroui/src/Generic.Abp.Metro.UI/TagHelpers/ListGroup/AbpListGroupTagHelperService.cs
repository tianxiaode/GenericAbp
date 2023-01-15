using Generic.Abp.Metro.UI.Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.ListGroup;

public class AbpListGroupTagHelperService : AbpTagHelperService<AbpListGroupTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "ul";
        output.Attributes.AddClass("list-group");

        if (TagHelper.Flush ?? false)
        {
            output.Attributes.AddClass("list-group-flush");
        }
    }
}
