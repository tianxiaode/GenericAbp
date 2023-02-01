using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Tab;

public class MetroTabsTagHelper : MetroTagHelper, IHasItems<TabItem>
{
    public bool Vertical { get; set; } = false;
    public bool Bottom { get; set; } = false;
    public bool Right { get; set; } = false;
    public TabAlignment Alignment { get; set; } = TabAlignment.Start;
    public TabType Type { get; set; } = TabType.Default;

    [HtmlAttributeName(TagHelperConsts.ItemsAttributeName)]
    public IEnumerable<TabItem> Items { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "ul";
        var attributes = output.Attributes;
        attributes.AddClass("tabs");
        await AddAttributesAsync(attributes);
        var child = await output.GetChildContentAsync();
        var items = Items ?? Enumerable.Empty<TabItem>();
        await AddItemsAsync(output, items);
        output.Content.AppendHtml(child);
    }

    protected virtual Task AddAttributesAsync(TagHelperAttributeList attributes)
    {
        attributes.Add("data-role", "tabs");
        attributes.Add("data-expand", "true");
        if (Vertical)
        {
            var clsVertical = "vertical";
            if (Right) clsVertical = $"{clsVertical} right";
            attributes.Add("data-tabs-position", clsVertical);
        }
        else
        {
            if (Bottom) attributes.Add("data-tabs-position", "bottom");
        }

        switch (Type)
        {
            case TabType.Text:
                attributes.Add("data-tabs-type", "text");
                break;
            case TabType.Group:
                attributes.Add("data-tabs-type", "group");
                break;
            case TabType.Pills:
                attributes.Add("data-tabs-type", "pills");
                break;
            case TabType.Default:
            default:
                break;
        }

        attributes.TryGetAttribute("data-cls-tabs", out var clsTabsAttribute);
        var alignment = Alignment switch
        {
            TabAlignment.Center => "flex-justify-center",
            TabAlignment.End => "flex-justify-end",
            _ => ""
        };
        if (clsTabsAttribute != null)
        {
            alignment = $"{clsTabsAttribute.Value} {alignment}";
            attributes.Remove(clsTabsAttribute);
        }

        attributes.Add("data-cls-tabs", alignment);

        return Task.CompletedTask;
    }

    public async Task AddItemsAsync(TagHelperOutput output, IEnumerable<TabItem> items)
    {
        var builder = new StringBuilder();
        foreach (var item in items)
        {
            var displayOrder = await GetDisplayOrderAsync(item.DisplayOrder);
            var active = item.Active ? "active" : "";
            builder.AppendLine(
                $"<li class=\"{active}\" style=\"order:{displayOrder}\"><a href=\"#{item.Target}\">{item.Title}</a></li>");
        }

        output.Content.AppendHtml(builder?.ToString());
    }
}