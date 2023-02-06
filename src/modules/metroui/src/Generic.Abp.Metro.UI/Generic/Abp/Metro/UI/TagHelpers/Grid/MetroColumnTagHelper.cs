using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers.Core;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Grid;

public class MetroColumnTagHelper : MetroTagHelper
{
    public ColumnSize Size { get; set; } = ColumnSize.Default;

    public ColumnSize SizeSm { get; set; } = ColumnSize.Default;

    public ColumnSize SizeMd { get; set; } = ColumnSize.Default;

    public ColumnSize SizeLg { get; set; } = ColumnSize.Default;

    public ColumnSize SizeXl { get; set; } = ColumnSize.Default;

    public ColumnSize Offset { get; set; } = ColumnSize.Default;

    public ColumnSize OffsetSm { get; set; } = ColumnSize.Default;

    public ColumnSize OffsetMd { get; set; } = ColumnSize.Default;

    public ColumnSize OffsetLg { get; set; } = ColumnSize.Default;

    public ColumnSize OffsetXl { get; set; } = ColumnSize.Default;

    public VerticalAlign VAlign { get; set; } = VerticalAlign.Default;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";

        output.Attributes.AddClass("cell");

        await ProcessSizeClassesAsync(context, output);
        await ProcessOffsetClassesAsync(context, output);
        await ProcessVerticalAlignAsync(output);
    }

    protected virtual async Task ProcessSizeClassesAsync(TagHelperContext context, TagHelperOutput output)
    {
        await ProcessSizeClassAsync(context, output, Size, null);
        await ProcessSizeClassAsync(context, output, SizeSm, MetroMediaMode.Sm);
        await ProcessSizeClassAsync(context, output, SizeMd, MetroMediaMode.Md);
        await ProcessSizeClassAsync(context, output, SizeLg, MetroMediaMode.Lg);
        await ProcessSizeClassAsync(context, output, SizeXl, MetroMediaMode.Xl);
    }

    protected virtual async Task ProcessOffsetClassesAsync(TagHelperContext context, TagHelperOutput output)
    {
        await ProcessOffsetClassAsync(context, output, Offset, null);
        await ProcessOffsetClassAsync(context, output, OffsetSm, MetroMediaMode.Sm);
        await ProcessOffsetClassAsync(context, output, OffsetMd, MetroMediaMode.Md);
        await ProcessOffsetClassAsync(context, output, OffsetLg, MetroMediaMode.Lg);
        await ProcessOffsetClassAsync(context, output, OffsetXl, MetroMediaMode.Xl);
    }

    protected virtual Task ProcessSizeClassAsync(TagHelperContext context, TagHelperOutput output, ColumnSize size,
        MetroMediaMode? mediaMode)
    {
        if (size == ColumnSize.Default)
        {
            return Task.CompletedTask;
        }

        var breakpoint = mediaMode != null ? "-" + mediaMode.ToString()?.ToLowerInvariant() : "";
        var classString = "cell" + breakpoint;

        if (size == ColumnSize.Auto)
        {
            classString += "-auto";
        }
        else if (size != ColumnSize._)
        {
            classString += "-" + size.ToString("D");
        }

        output.Attributes.RemoveClass("cell");
        output.Attributes.AddClass(classString);
        return Task.CompletedTask;
    }

    protected virtual Task ProcessOffsetClassAsync(TagHelperContext context, TagHelperOutput output, ColumnSize size,
        MetroMediaMode? mediaMode)
    {
        if (size == ColumnSize.Default)
        {
            return Task.CompletedTask;
        }

        var breakpoint = mediaMode != null ? "-" + mediaMode.ToString()?.ToLowerInvariant() : "";
        var classString = "offset" + breakpoint;

        if (size == ColumnSize._)
        {
            classString += "-0";
        }
        else
        {
            classString += "-" + size.ToString("D");
        }

        output.Attributes.AddClass(classString);
        return Task.CompletedTask;
    }

    protected virtual Task ProcessVerticalAlignAsync(TagHelperOutput output)
    {
        if (VAlign == VerticalAlign.Default)
        {
            return Task.CompletedTask;
        }

        output.Attributes.AddClass("flex-align-self-" + VAlign.ToString().ToLowerInvariant());
        return Task.CompletedTask;
    }
}