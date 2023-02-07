using System;
using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers.Core;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.NavigationView;

public class MetroNavViewTagHelper : MetroTagHelper
{
    public MetroMediaMode Compact { get; set; } = MetroMediaMode.Md;
    public MetroMediaMode Expand { get; set; } = MetroMediaMode.Lg;

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        var attributes = output.Attributes;
        attributes.Add("data-role", "navview");
        attributes.Add("data-compact", Enum.GetName(Compact)?.ToLowerInvariant());
        attributes.Add("data-expand", Enum.GetName(Expand)?.ToLowerInvariant());
        return Task.CompletedTask;
    }
}