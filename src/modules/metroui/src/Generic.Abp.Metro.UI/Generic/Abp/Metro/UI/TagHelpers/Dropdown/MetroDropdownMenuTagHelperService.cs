using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Dropdown;

public class MetroDropdownMenuTagHelperService : MetroTagHelperService<MetroDropdownMenuTagHelper>
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "ul";
        output.Attributes.Add("data-role","dropdown");
        output.TagMode = TagMode.StartTagAndEndTag;
        await AddClassesAsync(context, output);
    }

    protected virtual Task AddClassesAsync(TagHelperContext context, TagHelperOutput output)
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
        return Task.CompletedTask;
    }
}
