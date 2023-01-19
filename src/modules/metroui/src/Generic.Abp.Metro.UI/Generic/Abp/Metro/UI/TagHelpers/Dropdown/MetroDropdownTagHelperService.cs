using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Dropdown;

public class MetroDropdownTagHelperService : MetroTagHelperService<MetroDropdownTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.AddClass("pos-relative");

        output.TagMode = TagMode.StartTagAndEndTag;

    }

}
