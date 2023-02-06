using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Dropdown;

public class MetroDropdownTagHelper : MetroTagHelper
{
    public bool Split { get; set; } = false;

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (Split) context.Items[TagHelperConsts.DropdownIsSplitName] = true;
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.AddClass(Split ? "split-button" : "pos-relative");
        return Task.CompletedTask;
    }
}