using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Accordion;

public class MetroAccordionItemTagHelper : MetroTagHelper
{
    public string Title { get; set; }
    public bool Active { get; set; } = false;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.AddClass("frame");
        if (Active) output.Attributes.AddClass("active");
        var child = await output.GetChildContentAsync();
        output.Content.SetHtmlContent(await GetContentAsync(context, output, child));
    }

    protected virtual Task<string> GetContentAsync(TagHelperContext context, TagHelperOutput output,
        TagHelperContent content)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"<div class=\"heading\">{Title}</div>");
        stringBuilder.AppendLine($"<div class=\"content\">{content.GetContent()}</div>");
        return Task.FromResult(stringBuilder.ToString());
    }
}