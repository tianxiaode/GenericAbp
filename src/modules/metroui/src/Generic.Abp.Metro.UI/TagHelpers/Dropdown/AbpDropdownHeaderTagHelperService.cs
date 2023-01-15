using Generic.Abp.Metro.UI.Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Dropdown;

public class AbpDropdownHeaderTagHelperService : AbpTagHelperService<AbpDropdownHeaderTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "h6";
        output.Attributes.AddClass("dropdown-header");
        output.TagMode = TagMode.StartTagAndEndTag;
    }
}
