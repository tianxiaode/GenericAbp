using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Badge;

public class MetroBadgeTagHelper : MetroTagHelper
{
    public bool? Inside { get; set; }
    public bool? Inline { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "span";
        output.Attributes.AddClass("badge ");
        if (Inline == true) output.Attributes.AddClass("inline");
        if (Inside == true) output.Attributes.AddClass("Inside");
        return Task.CompletedTask;
    }
}