using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers.Menu;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Dropdown;

public class MetroDropdownMenuTagHelper : MetroMenuTagHelper
{
    public DropdownDirection Direction { get; set; } = DropdownDirection.Down;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        Type = MenuType.Dropdown;
        switch (Direction)
        {
            case DropdownDirection.Up:
                output.Attributes.AddClass("drop-up");
                break;
            case DropdownDirection.Left:
                output.Attributes.AddClass("drop-left");
                break;

            case DropdownDirection.Right:
                output.Attributes.AddClass("drop-right");
                break;
            case DropdownDirection.Down:
            default:
                break;
        }

        await base.ProcessAsync(context, output);
    }
}