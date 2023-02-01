using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.List;

[HtmlTargetElement("metro-list-item", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroListItemTagHelper : MetroListGroupTagHelperBase, IMetroListItem
{
    public string Title { get; set; }
    public string Image { get; set; }
    public string Label { get; set; }
    public string SecondLabel { get; set; }
    public string SecondAction { get; set; }
    public string Marker { get; set; }
    public string StepContent { get; set; }
    public int DisplayOrder { get; set; } = 0;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null;
        var builder = new StringBuilder();
        var order = DisplayOrder;
        await AddFeedTitleAsync(builder, Title, order);

        var item = await AddItemAsync(Title, Marker, StepContent, Image, Label, SecondLabel, SecondAction);
        await SetDisplayOrderAsync(item, order);
        builder.AppendLine(item.ToHtmlString());

        output.Content.AppendHtml(builder?.ToString());
    }
}