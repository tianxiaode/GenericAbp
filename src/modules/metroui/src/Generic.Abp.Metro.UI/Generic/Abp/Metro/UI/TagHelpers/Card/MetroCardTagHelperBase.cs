using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Card;

public abstract class MetroCardTagHelperBase : MetroTagHelper<CardGroupItem>
{
    protected MetroCardTagHelperBase(HtmlEncoder htmlEncoder) : base(htmlEncoder)
    {
    }

    public override void Init(TagHelperContext context)
    {
        GroupItemsName = TagHelperConsts.CardGroupItems;
    }

    protected virtual async Task<string> AddHeaderAsync<T>(T builder, string text, string image, string cls = "")
        where T : class
    {
        await AddClassAsync(builder, "card-header");
        await AppendHtmlAsync(builder, text);
        await AddClassAsync(builder, cls);
        if (!string.IsNullOrWhiteSpace(image))
        {
            await AddStyleAsync(builder, $"background-image: url({image});");
        }

        return await GetBuilderAsHtmlAsync(builder);
    }

    protected virtual async Task<string> AddContentAsync<T>(T builder, string content, string cls) where T : class
    {
        await AddClassAsync(builder, "card-content");
        await AddClassAsync(builder, cls);
        await AppendHtmlAsync(builder, content);

        return await GetBuilderAsHtmlAsync(builder);
    }

    protected virtual async Task<string> AddFooterAsync<T>(T builder, string text, string cls = "") where T : class
    {
        await AddClassAsync(builder, "card-footer");
        await AddClassAsync(builder, cls);
        await AppendHtmlAsync(builder, text);
        return await GetBuilderAsHtmlAsync(builder);
    }
}