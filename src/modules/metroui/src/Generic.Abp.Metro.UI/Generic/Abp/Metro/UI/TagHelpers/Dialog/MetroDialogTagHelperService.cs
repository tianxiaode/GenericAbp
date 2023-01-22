using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Dialog;

public class MetroDialogTagHelperService : MetroTagHelperService<MetroDialogTagHelper>
{
    public MetroDialogTagHelperService(IStringLocalizer<AbpUiResource> l, IHtmlGenerator htmlGenerator)
    {
        L = l;
        HtmlGenerator = htmlGenerator;
    }

    protected IStringLocalizer<AbpUiResource> L { get; }
    protected IHtmlGenerator HtmlGenerator { get; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await CreateItemsAsync<DialogItem>(context, DialogItems);
        output.TagName = "form";
        await AddAttributesAsync(output);
        var childContent = await output.GetChildContentAsync();
        output.Content.AppendHtml(await GetContentAsync(context, output, childContent.GetContent()));
    }
    
    protected virtual Task AddAttributesAsync(TagHelperOutput output)
    {
        var attributes = output.Attributes;
        attributes.AddClass("dialog");
        attributes.Add("data-role","dialog");
        if (!attributes.ContainsName("id")) attributes.Add("id", "{id}");
        return Task.CompletedTask;
    }

    protected virtual async Task<string> GetContentAsync(TagHelperContext context,TagHelperOutput output, string childContent)
    {
        var bodyHtml = await GetBodyAsync(context, childContent);
        var titleHtml = await GetTitleAsHtmlAsync(context, output);
        var footerHtml = await GetFooterAsHtmlAsync(context);
        return titleHtml + bodyHtml + footerHtml;
    }

    protected virtual async Task<string> GetBodyAsync(TagHelperContext context, string childContent)
    {
        if ( await IsTypeExistsAsync<DialogItem>(context, DialogItems, nameof(MetroDialogContentTagHelper)))
        {
            return "";
        };

        var tagBuilder = new TagBuilder("div");
        tagBuilder.AddCssClass("dialog-content");
        tagBuilder.AddCssClass("flex-fill");
        tagBuilder.AddCssClass("scroll-y");
        tagBuilder.InnerHtml.AppendHtml(childContent);
        return tagBuilder.ToHtmlString();

    }


    protected virtual async Task<string> GetTitleAsHtmlAsync(TagHelperContext context,TagHelperOutput output)
    {
        if (!output.Attributes.ContainsName("title")) return "";
        var attribute = output.Attributes["title"];
        var title = attribute.Value.ToString();
        output.Attributes.Remove(attribute);

        if ( await IsTypeExistsAsync<DialogItem>(context, DialogItems, nameof(MetroDialogTitleTagHelper)))
        {
            return "";
        };

        if (title.IsNullOrWhiteSpace()) return "";
        var tagBuilder = new TagBuilder("div");
        tagBuilder.AddCssClass("dialog-title");
        tagBuilder.InnerHtml.AppendHtml(title);
        return tagBuilder.ToHtmlString();
    }

    protected virtual async Task<string> GetFooterAsHtmlAsync(TagHelperContext context)
    {
        if (TagHelper.Buttons == null) return "";

        if ( await IsTypeExistsAsync<DialogItem>(context, DialogItems, nameof(MetroDialogFooterTagHelper)))
        {
            return "";
        };
        var tagBuilder = new TagBuilder("div");
        tagBuilder.AddCssClass("dialog-actions");
        var alignment = TagHelper.ButtonAlignment switch
        {
            ButtonsAlign.Center => "text-center",
            ButtonsAlign.End => "text-right",
            _ => ""
        };
        if (!alignment.IsNullOrWhiteSpace())
        {
            tagBuilder.AddCssClass(alignment);
        }

        tagBuilder.InnerHtml.AppendHtml(await CreateFooterContentAsync());
        return tagBuilder.ToHtmlString();
    }

    protected virtual async Task<string> CreateFooterContentAsync()
    {
        var sb = new StringBuilder();

        switch (TagHelper.Buttons)
        {
            case MetroDialogButtons.Cancel:
                sb.AppendLine(await CreteButtonAsync(L["Cancel"], "secondary", isClose:true));
                break;
            case MetroDialogButtons.Close:
                sb.AppendLine(await CreteButtonAsync(L["Close"], "secondary", isClose:true));
                break;
            case MetroDialogButtons.Submit:
                sb.AppendLine(await CreteButtonAsync(L["Save"], type:"submit"));
                break;
            case MetroDialogButtons.Submit | MetroDialogButtons.Cancel:
                sb.AppendLine(await CreteButtonAsync(L["Save"], type:"submit"));
                sb.AppendLine(await CreteButtonAsync(L["Cancel"], "secondary", isClose:true));
                break;
            case MetroDialogButtons.Submit | MetroDialogButtons.Close:
                sb.AppendLine(await CreteButtonAsync(L["Save"], type:"submit"));
                sb.AppendLine(await CreteButtonAsync(L["Close"], "secondary", isClose:true));
                break;
            case MetroDialogButtons.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return sb.ToString();
    }

    protected virtual Task<string> CreteButtonAsync(string text ,string color = "primary", string type = "button", string busyText="",bool isClose= false, string tagName = "button")
    {
        var builder = new TagBuilder(tagName);
        var attributes = builder.Attributes;
        attributes.Add("type", type);
        builder.AddCssClass("button");
        builder.AddCssClass(color);
        if (!busyText.IsNullOrWhiteSpace())
        {
            attributes.Add("data-busy-text", busyText);
        }
        if(isClose) builder.AddCssClass("js-dialog-close");

        var innerHtml = builder.InnerHtml;
        innerHtml.Append(text);
        return Task.FromResult(builder.ToHtmlString());
    }
}
