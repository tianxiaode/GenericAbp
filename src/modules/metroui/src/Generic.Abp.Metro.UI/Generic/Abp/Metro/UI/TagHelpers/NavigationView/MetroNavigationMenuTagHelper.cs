using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.NavigationView;

[HtmlTargetElement("metro-navigation-menu", Attributes = TagHelperConsts.ItemsAttributeName)]
public class MetroNavigationMenuTagHelper : MetroTagHelper, IHasItems<MetroNavigationMenuItem>
{
    [HtmlAttributeName(TagHelperConsts.ItemsAttributeName)]
    public IEnumerable<MetroNavigationMenuItem> Items { get; set; }

    public string CurrentValue { get; set; }

    public override void Init(TagHelperContext context)
    {
        context.Items[nameof(MetroNavigationMenuTagHelper)] = CurrentValue;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "ul";
        output.Attributes.AddClass("navview-menu");
        output.Attributes.AddClass("d-flex");
        output.Attributes.AddClass("flex-column");
        var child = await output.GetChildContentAsync();
        var items = Items ?? Enumerable.Empty<MetroNavigationMenuItem>();
        await AddItemsAsync(output, items);
        output.Content.AppendHtml(child);
    }

    public async Task AddItemsAsync(TagHelperOutput output, IEnumerable<MetroNavigationMenuItem> items)
    {
        var builder = new StringBuilder();
        foreach (var item in items)
        {
            var displayOrder = await GetDisplayOrderAsync(item.DisplayOrder);

            switch (item.Type)
            {
                case MetroNavigationMenuItemType.Separator:
                    builder.Append($"<li class=\"item-separator\" style=\"order:{displayOrder}\"></li>");
                    continue;
                case MetroNavigationMenuItemType.Header:
                    builder.Append($"<li class=\"item-header\" style=\"order:{displayOrder}\">{item.Text}</li>");
                    continue;
                case MetroNavigationMenuItemType.Default:
                default:
                    var active = "";
                    if (!string.IsNullOrWhiteSpace(item.Value))
                    {
                        active = item.Value.Equals(CurrentValue, StringComparison.OrdinalIgnoreCase)
                            ? "active"
                            : "";
                    }

                    builder.Append(
                        $"<li class=\"{active}\" style=\"order:{displayOrder}\">");
                    builder.Append($"<a href=\"{item.Url}\">");
                    if (!string.IsNullOrWhiteSpace(item.Icon))
                    {
                        builder.Append($"<span class=\"icon\"><span class=\"{item.Icon}\"></span></span>");
                    }

                    builder.Append($" <span class=\"caption\">{item.Text}</span>");
                    builder.Append("</a></li>");
                    continue;
            }
        }

        output.Content.AppendHtml(builder?.ToString());
    }
}