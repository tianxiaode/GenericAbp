using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.NavigationView;

[HtmlTargetElement("metro-navigation-menu-item", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroNavigationMenuItemTagHelper : MetroNavigationMenuTagHelperBase, IMetroNavigationMenuItem
{
    public string Text { get; set; }
    public string Icon { get; set; }
    public string Url { get; set; }
    public string Value { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public MetroNavigationMenuItemType Type { get; set; } = MetroNavigationMenuItemType.Default;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null;
        var currentValue = (string)context.Items[nameof(MetroNavigationMenuTagHelper)];
        var tagBuilder = await AddItemAsync(Type, Text, Value, Url, Icon, currentValue);
        await SetDisplayOrderAsync(tagBuilder, DisplayOrder);
        output.Content.AppendHtml(tagBuilder.ToHtmlString());
    }
}