using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Grid;

public class MetroGridTagHelper : MetroTagHelper
{
    public bool Gap { get; set; } = true;

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.AddClass("grid");
        if (!Gap) output.Attributes.AddClass("no-gap");
        return Task.CompletedTask;
    }
}