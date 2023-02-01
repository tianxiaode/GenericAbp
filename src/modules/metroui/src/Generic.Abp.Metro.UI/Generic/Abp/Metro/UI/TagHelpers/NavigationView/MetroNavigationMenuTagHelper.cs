using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.NavigationView;

public class MetroNavigationMenuTagHelper : MetroNavigationMenuTagHelperBase, IHasItems<MetroNavigationMenuItem>
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

            var tagBuilder = await AddItemAsync(item.Type, item.Text, item.Value, item.Url, item.Icon, CurrentValue);
            await SetDisplayOrderAsync(tagBuilder, displayOrder);
            builder.AppendLine(tagBuilder.ToHtmlString());
        }

        output.Content.AppendHtml(builder?.ToString());
    }
}