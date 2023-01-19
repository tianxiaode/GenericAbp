using System;
using System.Text;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;

namespace Generic.Abp.Metro.UI.TagHelpers.Dialog;

public class MetroDialogFooterTagHelperService : MetroTagHelperService<MetroDialogFooterTagHelper>
{
    private readonly IStringLocalizer<AbpUiResource> _localizer;

    public MetroDialogFooterTagHelperService(IStringLocalizer<AbpUiResource> localizer)
    {
        _localizer = localizer;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        AddClasses(context, output);

        if (TagHelper.Buttons != MetroDialogButtons.None)
        {
            output.PostContent.SetHtmlContent(CreateContent());
        }

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

    protected virtual string CreateContent()
    {
        var sb = new StringBuilder();

        switch (TagHelper.Buttons)
        {
            case MetroDialogButtons.Cancel:
                sb.AppendLine(GetCancelButton());
                break;
            case MetroDialogButtons.Close:
                sb.AppendLine(GetCloseButton());
                break;
            case MetroDialogButtons.Submit:
                sb.AppendLine(GetSubmitButton());
                break;
            case MetroDialogButtons.Submit | MetroDialogButtons.Cancel:
                sb.AppendLine(GetSubmitButton());
                sb.AppendLine(GetCancelButton());
                break;
            case MetroDialogButtons.Submit | MetroDialogButtons.Close:
                sb.AppendLine(GetSubmitButton());
                sb.AppendLine(GetCloseButton());
                break;
            case MetroDialogButtons.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return sb.ToString();
    }

    protected virtual string GetSubmitButton()
    {
        //var icon = new TagBuilder("span");
        //icon.AddCssClass("mif-checkmark");

        //var span = new TagBuilder("span");
        //span.InnerHtml.Append(_localizer["Save"]);

        var element = new TagBuilder("button");
        element.Attributes.Add("type", "submit");
        element.AddCssClass("button");
        element.AddCssClass("primary");
        element.Attributes.Add("data-busy-text", _localizer["SavingWithThreeDot"]);
        //element.InnerHtml.AppendHtml(icon);
        element.InnerHtml.Append(_localizer["Save"]);

        return element.ToHtmlString();
    }

    protected virtual string GetCloseButton()
    {
        var element = new TagBuilder("button");
        element.Attributes.Add("type", "button");
        element.AddCssClass("button");
        element.AddCssClass("js-dialog-close");
        element.AddCssClass("secondary");
        element.InnerHtml.Append(_localizer["Close"]);

        return element.ToHtmlString();
    }

    protected virtual string GetCancelButton()
    {
        var element = new TagBuilder("button");
        element.Attributes.Add("type", "button");
        element.AddCssClass("button");
        element.AddCssClass("js-dialog-close");
        element.AddCssClass("secondary");
        element.InnerHtml.Append(_localizer["Cancel"]);

        return element.ToHtmlString();
    }

}
