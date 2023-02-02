using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Card;

public class MetroCardTagHelper : MetroCardTagHelperBase
{
    public string Header { get; set; }
    public string HeaderImage { get; set; }
    public string Content { get; set; }
    public string ContentCls { get; set; }
    public string Footer { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.AddClass("card");
        output.Content.AppendHtml(await GetContentAsync(context, output));
        output.Content.AppendHtml(await output.GetChildContentAsync());
    }

    protected virtual async Task<string> GetContentAsync(TagHelperContext context, TagHelperOutput output)
    {
        var builder = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(Header))
        {
            var header = new TagBuilder("div");
            await AddHeaderAsync(header, Header, HeaderImage);
            builder.AppendLine(header.ToHtmlString());
        }

        if (!string.IsNullOrWhiteSpace(Content))
        {
            var content = new TagBuilder("div");
            await AddContentAsync(content, Content);
            await AddClassAsync(content, ContentCls);
            builder.AppendLine(content.ToHtmlString());
        }

        if (string.IsNullOrWhiteSpace(Footer)) return builder.ToString();
        var footer = new TagBuilder("div");
        await AddFooterAsync(footer, Footer);
        builder.AppendLine(footer.ToHtmlString());

        return builder.ToString();
    }
}