using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Dropdown;

public class MetroDropdownTagHelper : MetroTagHelper
{
    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.AddClass("pos-relative");
        output.PreElement.SetHtmlContent(
            $"<div>{JsonSerializer.Serialize(context.Items.Keys.Select(m => m.ToString()))}</div>");
        return Task.CompletedTask;
    }
}