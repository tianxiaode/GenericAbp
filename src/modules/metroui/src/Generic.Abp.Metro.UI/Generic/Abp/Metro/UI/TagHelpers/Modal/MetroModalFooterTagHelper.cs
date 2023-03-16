using System.Text;
using System;
using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;

namespace Generic.Abp.Metro.UI.TagHelpers.Modal;

[HtmlTargetElement("metro-modal-footer", TagStructure = TagStructure.WithoutEndTag)]
public class MetroModalFooterTagHelper : MetroTagHelper
{
    public MetroModalFooterTagHelper(IStringLocalizer<AbpUiResource> l)
    {
        L = l;
    }

    protected IStringLocalizer<AbpUiResource> L { get; }
    public ModalDialogButtons Buttons { get; set; }
    public ModalButtonsAlign ButtonAlignment { get; set; } = ModalButtonsAlign.Default;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        await AddClassesAsync(context, output);

        if (Buttons != ModalDialogButtons.None)
        {
            output.PostContent.SetHtmlContent(await CreateFooterContentAsync());
        }
    }

    protected virtual Task AddClassesAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.AddClass("dialog-actions");
        var alignment = ButtonAlignment switch
        {
            ModalButtonsAlign.Center => "text-center",
            ModalButtonsAlign.End => "text-right",
            _ => ""
        };
        if (!alignment.IsNullOrWhiteSpace())
        {
            output.Attributes.AddClass(alignment);
        }

        return Task.CompletedTask;
    }

    protected virtual async Task<string> CreateFooterContentAsync()
    {
        var sb = new StringBuilder();

        switch (Buttons)
        {
            case ModalDialogButtons.Cancel:
                sb.AppendLine(await CreteButtonAsync(L["Cancel"], "secondary", isClose: true));
                break;
            case ModalDialogButtons.Close:
                sb.AppendLine(await CreteButtonAsync(L["Close"], "secondary", isClose: true));
                break;
            case ModalDialogButtons.Save:
                sb.AppendLine(await CreteButtonAsync(L["Save"], isSave: true));
                break;
            case ModalDialogButtons.Save | ModalDialogButtons.Cancel:
                sb.AppendLine(await CreteButtonAsync(L["Save"], isSave: true));
                sb.AppendLine(await CreteButtonAsync(L["Cancel"], "secondary", isClose: true));
                break;
            case ModalDialogButtons.Save | ModalDialogButtons.Close:
                sb.AppendLine(await CreteButtonAsync(L["Save"], isSave: true));
                sb.AppendLine(await CreteButtonAsync(L["Close"], "secondary", isClose: true));
                break;
            case ModalDialogButtons.None:
            default:
                break;
        }

        return sb.ToString();
    }

    protected virtual Task<string> CreteButtonAsync(string text, string color = "primary", bool isClose = false,
        bool isSave = false)
    {
        var builder = new TagBuilder("button");
        var attributes = builder.Attributes;
        builder.AddCssClass("button");
        builder.AddCssClass(color);

        if (isClose) builder.AddCssClass("js-dialog-close");
        if (isSave) builder.AddCssClass("js-dialog-save");

        var innerHtml = builder.InnerHtml;
        innerHtml.Append(text);
        return Task.FromResult(builder.ToHtmlString());
    }
}