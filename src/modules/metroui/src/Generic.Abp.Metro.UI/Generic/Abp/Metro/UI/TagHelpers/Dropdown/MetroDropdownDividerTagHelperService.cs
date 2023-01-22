using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Dropdown;

public class MetroDropdownDividerTagHelperService : MetroTagHelperService<MetroDropdownDividerTagHelper>
{
    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "li";
        output.Attributes.AddClass("divider");
        output.TagMode = TagMode.StartTagAndEndTag;
        return Task.CompletedTask;
    }
}
