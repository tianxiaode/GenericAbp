using Generic.Abp.Metro.UI.TagHelpers.Menu;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.NavigationView;

[HtmlTargetElement("metro-nav-menu-item", ParentTag = "metro-nav-menu", TagStructure = TagStructure.WithoutEndTag)]
public class MetroNavMenuItemTagHelper : MenuItemTagHelperBase
{
    public string Text { get; set; }
    public string Icon { get; set; }
    public string Href { get; set; }
    public string Value { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var currentValue = (string)context.Items[nameof(MetroNavMenuTagHelper)];
        await SetMainTagAsync(context, output, isActive: currentValue == Value);
        var iconHtml = await GetIconAsHtmlAsync(Icon, true);
        var anchorHtml = await GetAnchorAsHtmlAsync(Href, caption: Text, icon: iconHtml);
        output.Content.AppendHtml(anchorHtml);
    }
}