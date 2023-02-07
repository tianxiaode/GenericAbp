using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers.Core;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.SideNav;

public class MetroSideNavTagHelper : MetroTagHelper
{
    public SideNavType Type { get; set; } = SideNavType.M3;
    public MetroMediaMode? Expand { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var type = Type.ToString().ToLowerInvariant();
        context.Items[nameof(MetroSideNavTagHelper)] = type;
        output.TagName = "ul";
        output.Attributes.AddClass($"sidenav-{type}");
        if (Expand != null)
        {
            output.Attributes.AddClass($"sidenav-{type}-expand-{Expand.ToString()?.ToLowerInvariant()}");
        }

        return base.ProcessAsync(context, output);
    }
}