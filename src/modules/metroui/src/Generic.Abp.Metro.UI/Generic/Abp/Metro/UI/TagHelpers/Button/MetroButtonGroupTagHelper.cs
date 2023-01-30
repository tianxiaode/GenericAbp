using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

public class MetroButtonGroupTagHelper : MetroTagHelper
{
    public bool? Multi { get; set; }
    public string ActiveCls { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.Add("data-role", "buttongroup");
        if (Multi == true) output.Attributes.Add("data-mode", "multi");
        if (!string.IsNullOrWhiteSpace(ActiveCls))
        {
            output.Attributes.Add("data-cls-active", ActiveCls);
        }

        return Task.CompletedTask;
    }
}