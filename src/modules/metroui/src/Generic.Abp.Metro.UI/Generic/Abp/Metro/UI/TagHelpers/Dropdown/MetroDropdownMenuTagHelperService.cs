using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Dropdown;

public class MetroDropdownMenuTagHelperService : MetroTagHelperService<MetroDropdownMenuTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "ul";
        output.Attributes.Add("data-role","dropdown");
        output.TagMode = TagMode.StartTagAndEndTag;
        AddClasses(context, output);
    }

    protected virtual void AddClasses(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.AddClass("d-menu");

        var dir = TagHelper.Direction switch
        {
            DropdownDirection.Left => "drop-left",
            DropdownDirection.Up => "drop-up",
            DropdownDirection.Right => "drop-right",
            _ => ""
        };

        output.Attributes.AddClass(dir);

    }
}
