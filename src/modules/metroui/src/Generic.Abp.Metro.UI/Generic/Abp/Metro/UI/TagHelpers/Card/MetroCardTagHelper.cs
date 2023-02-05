using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Card;

public class MetroCardTagHelper : MetroCardTagHelperBase
{
    public MetroCardTagHelper(HtmlEncoder htmlEncoder) : base(htmlEncoder)
    {
        GroupItemsName = TagHelperConsts.CardGroupItems;
    }

    public string Header { get; set; }
    public string HeaderImage { get; set; }
    public string HeaderCls { get; set; }
    public string Content { get; set; }
    public string ContentCls { get; set; }
    public string Footer { get; set; }
    public string FooterCls { get; set; }


    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var list = await InitGroupItemsAsync(context);
        output.TagName = "div";
        output.Attributes.AddClass("card");
        if (!string.IsNullOrEmpty(HeaderImage)) output.Attributes.AddClass("image-header");
        var child = await output.GetChildContentAsync();
        output.Content.AppendHtml(await GetContentAsync(context, output, child, list));
    }

    protected virtual async Task<string> GetContentAsync(TagHelperContext context, TagHelperOutput output,
        TagHelperContent child, List<CardGroupItem> list)
    {
        var builder = new StringBuilder();
        var header = await GetGroupItemAsync(context, nameof(MetroCardHeaderTagHelper));
        if (header != null)
        {
            builder.AppendLine(header.HtmlContent);
            if (header.HtmlContent.Contains("background-image")) output.Attributes.AddClass("image-header");
        }
        else
        {
            builder.AppendLine(await AddHeaderAsync(new TagBuilder("div"), Header, HeaderImage, HeaderCls));
        }

        var content = list.FirstOrDefault(m => m.Name == nameof(MetroCardContentTagHelper));
        if (content != null)
        {
            builder.AppendLine(content.HtmlContent);
        }
        else
        {
            builder.AppendLine(await AddContentAsync(new TagBuilder("div"),
                Content ?? child.GetContent(), ContentCls));
        }

        var footer = await GetGroupItemAsync(context, nameof(MetroCardFooterTagHelper));
        if (footer != null)
        {
            builder.AppendLine(footer.HtmlContent);
        }
        else
        {
            builder.AppendLine(await AddFooterAsync(new TagBuilder("div"), Footer, FooterCls));
        }

        return builder.ToString();
    }
}