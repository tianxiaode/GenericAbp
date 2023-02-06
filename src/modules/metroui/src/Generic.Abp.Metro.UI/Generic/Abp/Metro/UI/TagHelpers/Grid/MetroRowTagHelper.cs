using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Grid;

public class MetroRowTagHelper : MetroTagHelper
{
    public VerticalAlign VAlign { get; set; } = VerticalAlign.Default;
    public HorizontalAlign HAlign { get; set; } = HorizontalAlign.Default;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.AddClass("row");

        await ProcessVerticalAlignAsync(output);
        await ProcessHorizontalAlignAsync(output);
    }

    protected virtual Task ProcessVerticalAlignAsync(TagHelperOutput output)
    {
        if (VAlign == VerticalAlign.Default)
        {
            return Task.CompletedTask;
        }

        output.Attributes.AddClass("flex-align-" + VAlign.ToString().ToLowerInvariant());
        return Task.CompletedTask;
    }

    protected virtual Task ProcessHorizontalAlignAsync(TagHelperOutput output)
    {
        if (HAlign == HorizontalAlign.Default)
        {
            return Task.CompletedTask;
        }

        output.Attributes.AddClass("flex-justify-" + HAlign.ToString().ToLowerInvariant());
        return Task.CompletedTask;
    }
}