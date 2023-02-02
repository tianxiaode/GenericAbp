using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Card;

public abstract class MetroCardTagHelperBase : MetroTagHelper
{
    protected virtual async Task AddHeaderAsync<T>(T builder, string text, string image) where T : class
    {
        await AddClassAsync(builder, "card-header");
        await AppendHtmlAsync(builder, text);
        if (!string.IsNullOrWhiteSpace(image))
        {
            await AddAttributeAsync(builder, "style", $"background-image: url({image})");
        }
    }

    protected virtual async Task AddContentAsync<T>(T builder, string content) where T : class
    {
        await AddClassAsync(builder, "card-content");
        await AppendHtmlAsync(builder, content);
    }

    protected virtual async Task AddFooterAsync<T>(T builder, string text) where T : class
    {
        await AddClassAsync(builder, "card-footer");
        await AppendHtmlAsync(builder, text);
    }
}