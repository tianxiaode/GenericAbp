using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.List;

public class MetroListGroupTagHelper : MetroListGroupTagHelperBase, IHasItems<MetroListItem>
{
    public bool Horizontal { get; set; } = false;

    [HtmlAttributeName(TagHelperConsts.ItemsAttributeName)]
    public IEnumerable<MetroListItem> Items { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "ul";
        await AddClassAsync(output);
        var child = await output.GetChildContentAsync();
        var items = Items ?? Enumerable.Empty<MetroListItem>();
        await AddItemsAsync(output, items);
        output.Content.AppendHtml(child);
    }

    protected virtual Task AddClassAsync(TagHelperOutput output)
    {
        var cls = Type switch
        {
            ListType.Feed => "feed-list",
            ListType.Step => "step-list",
            ListType.Marker => "custom-list-marker",
            ListType.Items => "items-list",
            _ => "group-list",
        };
        output.Attributes.AddClass(cls);
        output.Attributes.AddClass(Horizontal ? "horizontal" : "d-flex flex-column");
        return Task.CompletedTask;
    }

    public virtual async Task AddItemsAsync(TagHelperOutput output, IEnumerable<MetroListItem> items)
    {
        var builder = new StringBuilder();
        foreach (var item in items)
        {
            var displayOrder = await GetDisplayOrderAsync();
            await AddFeedTitleAsync(builder, item.Title, displayOrder);

            var tagBuilder = await AddItemAsync(item.Title, item.Marker, item.StepContent, item.Image, item.Label,
                item.SecondLabel, item.SecondAction);
            await SetDisplayOrderAsync(tagBuilder, displayOrder);
            //tagBuilder.Attributes.Add("style", $"order:{displayOrder}");
            builder.AppendLine(tagBuilder.ToHtmlString());
        }

        output.Content.AppendHtml(builder?.ToString());
    }
}