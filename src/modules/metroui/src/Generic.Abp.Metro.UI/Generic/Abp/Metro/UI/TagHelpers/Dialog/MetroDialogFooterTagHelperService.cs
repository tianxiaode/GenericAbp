using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Dialog;

public class MetroDialogFooterTagHelperService : MetroTagHelperService<MetroDialogFooterTagHelper>
{
    protected IStringLocalizer<AbpUiResource> L { get; }

    public MetroDialogFooterTagHelperService(IStringLocalizer<AbpUiResource> localizer)
    {
        L = localizer;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        AddClasses(context, output);

        if (TagHelper.Buttons != MetroDialogButtons.None)
        {
            output.PostContent.SetHtmlContent(await CreateFooterContentAsync());
        }

        await AddItemToItemsAsync<DialogItem>(context, DialogItems, nameof(MetroDialogFooterTagHelper));
    }

    protected virtual void AddClasses(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.AddClass("dialog-actions");
        var alignment = TagHelper.ButtonAlignment switch
        {
            ButtonsAlign.Center => "text-center",
            ButtonsAlign.End => "text-right",
            _ => ""
        };
        if (!alignment.IsNullOrWhiteSpace())
        {
            output.Attributes.AddClass(alignment);
        }
        if (TagHelper.ButtonAlignment == ButtonsAlign.Default)
        {
            return;
        }

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
