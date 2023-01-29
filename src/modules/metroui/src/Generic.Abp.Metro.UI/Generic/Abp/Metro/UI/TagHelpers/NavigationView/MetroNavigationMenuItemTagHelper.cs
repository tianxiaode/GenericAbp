using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.NavigationView;

[HtmlTargetElement("metro-navigation-menu-item", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroNavigationMenuItemTagHelper : MetroTagHelper, IMetroNavigationMenuItem
{
    public string Text { get; set; }
    public MetroNavigationMenuItemType Type { get; set; }
    public string Icon { get; set; }
    public string Url { get; set; }
    public string Value { get; set; }
    public int DisplayOrder { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "li";
        switch (Type)
        {
            case MetroNavigationMenuItemType.Separator:
                output.Attributes.AddClass("item-separator");
                break;
            case MetroNavigationMenuItemType.Header:
                output.Attributes.AddClass("item-header");
                output.Content.AppendHtml(Text);
                await SetDisplayOrderAsync(context, output);
                break;
            case MetroNavigationMenuItemType.Default:
            default:
                await SetCurrentValueAsync(context, output);
                await SetDisplayOrderAsync(context, output);
                await ProcessItemAsync(context, output);
                break;
        }
    }

    protected virtual Task SetDisplayOrderAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (DisplayOrder == 0) return Task.CompletedTask;
        output.Attributes.Add("style", $"order:{DisplayOrder}");
        return Task.CompletedTask;
    }

    protected virtual Task SetCurrentValueAsync(TagHelperContext context, TagHelperOutput output)
    {
        var currentValue = (string)context.Items[nameof(MetroNavigationMenuTagHelper)];
        if (string.IsNullOrWhiteSpace(Value)) return Task.CompletedTask;
        if (Value.Equals(currentValue, StringComparison.OrdinalIgnoreCase)) output.Attributes.AddClass("active");
        return Task.CompletedTask;
    }

    protected virtual Task ProcessItemAsync(TagHelperContext context, TagHelperOutput output)
    {
        var builder = new StringBuilder();

        builder.Append($"<a href=\"{Url}\">");
        if (!string.IsNullOrWhiteSpace(Icon))
        {
            builder.Append($"<span class=\"icon\"><span class=\"{Icon}\"></span></span>");
        }

        builder.Append($" <span class=\"caption\">{Text}</span>");
        builder.Append("</a>");
        output.Content.AppendHtml(builder?.ToString());
        return Task.CompletedTask;
    }
}