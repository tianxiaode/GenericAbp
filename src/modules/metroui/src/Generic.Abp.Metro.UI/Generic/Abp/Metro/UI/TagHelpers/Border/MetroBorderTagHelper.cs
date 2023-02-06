using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text;
using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers.Core;

namespace Generic.Abp.Metro.UI.TagHelpers.Border;

[HtmlTargetElement(Attributes = "border")]
public class MetroBorderTagHelper : MetroTagHelper
{
    [HtmlAttributeName("border")] public BorderType BorderType { get; set; }

    public int BorderSize { get; set; } = 1;
    public MetroColor BorderColor { get; set; } = MetroColor.Gray;
    public string CustomBorderColor { get; set; }
    public BorderStyle BorderStyle { get; set; } = BorderStyle.Solid;
    public BorderRadius BorderRadius { get; set; } = BorderRadius.None;
    public string CustomBorderRadius { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await AddBorderStyleAsync(output);
        await AddBorderClassAsync(output);
    }

    protected virtual async Task AddBorderStyleAsync(TagHelperOutput output)
    {
        var name = Enum.GetName(BorderType);
        if (string.IsNullOrWhiteSpace(name)) return;
        var style = $"{BorderSize}px {Enum.GetName(BorderStyle)?.ToLowerInvariant()}";

        var builder = new StringBuilder();
        if (name.Contains("LRTB"))
        {
            builder.Append($"border:{style};");
        }
        else
        {
            if (name.Contains("L")) builder.Append($"border-left:{style};");
            if (name.Contains("R")) builder.Append($"border-right:{style};");
            if (name.Contains("T")) builder.Append($"border-top:{style};");
            if (name.Contains("B")) builder.Append($"border-bottom:{style};");
        }

        if (!string.IsNullOrWhiteSpace(CustomBorderColor))
        {
            builder.Append($"border-color:{CustomBorderColor};");
        }

        if (!string.IsNullOrWhiteSpace(CustomBorderRadius))
        {
            builder.Append($"border-radius:{CustomBorderRadius};");
        }

        await AddStyleAsync(output, builder.ToString());
        output.Attributes.Add("style", builder.ToString());
    }

    protected virtual Task AddBorderClassAsync(TagHelperOutput output)
    {
        var attributes = output.Attributes;
        attributes.AddClass($"bd-{Enum.GetName(BorderColor)?.ToLowerInvariant()}");

        switch (BorderRadius)
        {
            case BorderRadius.Default:
                attributes.AddClass("border-radius");
                return Task.CompletedTask;
            case BorderRadius.Half:
            case BorderRadius._1:
            case BorderRadius._2:
            case BorderRadius._3:
            case BorderRadius._4:
            case BorderRadius._5:
            case BorderRadius._6:
            case BorderRadius._7:
            case BorderRadius._8:
            case BorderRadius._9:
            case BorderRadius._10:
                var name = Enum.GetName(BorderRadius);
                if (string.IsNullOrWhiteSpace(name)) return Task.CompletedTask;
                output.Attributes.AddClass($"border-radius-{name.ToLowerInvariant().Replace("_", "")}");
                break;
            case BorderRadius.None:
            default:
                break;
        }

        return Task.CompletedTask;
    }
}