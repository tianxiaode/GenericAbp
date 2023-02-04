using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers.Button;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Dropdown;

public class MetroDropdownButtonTagHelper : MetroButtonTagHelper
{
    public bool Split { get; set; } = false;

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.AddClass("dropdown-toggle");
        if (Split) output.Attributes.AddClass("split");
        return base.ProcessAsync(context, output);
    }
}