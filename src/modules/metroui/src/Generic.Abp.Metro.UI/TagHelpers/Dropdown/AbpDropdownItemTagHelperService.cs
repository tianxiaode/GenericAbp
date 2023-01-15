using Generic.Abp.Metro.UI.Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Dropdown;

public class AbpDropdownItemTagHelperService : AbpTagHelperService<AbpDropdownItemTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "a";
        output.Attributes.AddClass("dropdown-item");
        output.TagMode = TagMode.StartTagAndEndTag;

        SetActiveClassIfActive(context, output);
        SetDisabledClassIfDisabled(context, output);
    }

    protected virtual void SetActiveClassIfActive(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.Active ?? false)
        {
            output.Attributes.AddClass("active");
        }
    }

    protected virtual void SetDisabledClassIfDisabled(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.Disabled ?? false)
        {
            output.Attributes.AddClass("disabled");
        }
    }
}
