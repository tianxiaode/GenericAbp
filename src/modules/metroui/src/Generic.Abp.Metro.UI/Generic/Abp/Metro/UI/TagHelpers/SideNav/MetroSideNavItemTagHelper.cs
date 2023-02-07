using System;
using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers.Core;
using Generic.Abp.Metro.UI.TagHelpers.Menu;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.SideNav;

[HtmlTargetElement("metro-side-nav-item", ParentTag = "metro-side-nav",
    TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroSideNavItemTagHelper : MenuItemTagHelperBase
{
    public string Href { get; set; }
    public string Icon { get; set; }
    public string Text { get; set; }
    public bool DropDown { get; set; } = false;
    public bool Active { get; set; } = false;
    public bool StickLeft { get; set; } = false;
    public MetroColor? StickColor { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var type = context.Items[nameof(MetroSideNavTagHelper)].ToString();
        var counter = type == "counter" ? "0" : "";
        await SetMainTagAsync(context, output, isActive: Active);
        var iconHtml = await GetIconAsHtmlAsync(Icon);
        var anchorHtml = "";
        if (type == "m3")
        {
            if (Active)
            {
                output.Attributes.AddClass(StickLeft ? "stick-left" : "stick-right");
                if (StickColor != null)
                {
                    var color = StickColor.Value.ToString();
                    color = color[..1].ToLowerInvariant() + color[1..];
                    output.Attributes.AddClass($"bg-{color}");
                }
            }

            anchorHtml = await GetAnchorAsHtmlAsync(Href, icon: iconHtml, text: Text, counter: counter,
                isDropDown: DropDown);
        }
        else
        {
            anchorHtml = await GetAnchorAsHtmlAsync(Href, icon: iconHtml, title: Text, counter: counter,
                isDropDown: DropDown);
        }

        var child = await output.GetChildContentAsync();
        output.Content.AppendHtml(anchorHtml);
        output.Content.AppendHtml(child);
    }
}