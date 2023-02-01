using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Breadcrumb;

public class MetroBreadcrumbTagHelper : MetroTagHelper<BreadcrumbGroupItem>, IHasItems<MetroBreadcrumbItem>
{
    [HtmlAttributeName(TagHelperConsts.ItemsAttributeName)]
    public IEnumerable<MetroBreadcrumbItem> Items { get; set; }

    public override void Init(TagHelperContext context)
    {
        GroupItemsName = TagHelperConsts.BreadcrumbItems;
        InitGroupItems(context);
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "ul";
        output.Attributes.AddClass("breadcrumbs");
        var child = await output.GetChildContentAsync();
        var items = Items ?? Enumerable.Empty<MetroBreadcrumbItem>();
        var groupItems = await GetGroupItems(context) ?? new List<BreadcrumbGroupItem>();
        //output.Content.AppendHtml(child);
        await AddItemsAsync(output, items, groupItems);
    }

    public async Task AddItemsAsync(TagHelperOutput output, IEnumerable<MetroBreadcrumbItem> items,
        List<BreadcrumbGroupItem> groupItems)
    {
        foreach (var item in Items)
        {
            var displayOrder = await GetDisplayOrderAsync(item.DisplayOrder);
            var html =
                $"<li class=\"page-item\" style=\"order:{displayOrder}\"><a href=\"{item.Url}\" class=\"page-link {item.Cls}\">{item.Title}</a></li>";
            await AddGroupItemAsync(groupItems, item.Title, displayOrder, html);
        }

        var stringBuilder = new StringBuilder("");
        foreach (var item in groupItems.OrderBy(m => m.DisplayOrder))
        {
            stringBuilder.AppendLine(item.HtmlContent);
        }

        output.Content.AppendHtml(stringBuilder.ToString());
    }
}