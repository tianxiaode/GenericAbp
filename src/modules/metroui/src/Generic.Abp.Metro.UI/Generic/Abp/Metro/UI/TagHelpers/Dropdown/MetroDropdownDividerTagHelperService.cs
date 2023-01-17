using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Dropdown;

public class MetroDropdownDividerTagHelperService : MetroTagHelperService<MetroDropdownDividerTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "li";
        output.Attributes.AddClass("divider");
        output.TagMode = TagMode.StartTagAndEndTag;
    }
}
