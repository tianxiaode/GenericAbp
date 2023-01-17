using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Dropdown;

public class MetroDropdownItemTagHelperService : MetroTagHelperService<MetroDropdownItemTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "li";
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
