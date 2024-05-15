using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq.Expressions;

namespace Generic.Abp.Tailwind.Microsoft.AspNetCore.Mvc.Rendering;

public static class HtmlHelperInputExtensions
{
    public static IHtmlContent TailwindInputFor<TModel, TResult>(
        this IHtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TResult>> expression,
        InputOptions inputOptions)
    {
        ArgumentNullException.ThrowIfNull(htmlHelper);
        ArgumentNullException.ThrowIfNull(expression);

        var divBuilder = new TagBuilder("div");
        divBuilder.AddCssClass("grid grid-cols-12 w-full gap-1 mb-4 input-group");

        var innerHtml = divBuilder.InnerHtml;
        HtmlContentUtilities.AddAttributes(divBuilder, inputOptions.FormControlAttributes);


        if (inputOptions.LabelCols > 0)
        {
            innerHtml.AppendHtml(CreateLabel(htmlHelper, expression, inputOptions));
        }

        innerHtml.AppendHtml(CreateInput(htmlHelper, expression, inputOptions));

        if (inputOptions.Type == InputType.Password)
        {
            CreatePasswordIndicator(innerHtml, inputOptions);
        }

        CreateErrorWarp(innerHtml, inputOptions);

        return divBuilder;
        ;
    }

    public static IHtmlContent CreateLabel<TModel, TResult>(IHtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TResult>> expression, InputOptions inputOptions)
    {
        var builder = new TagBuilder("label");
        builder.AddCssClass($"label col-span-{inputOptions.LabelCols} ");
        builder.Attributes.Add("for", htmlHelper.IdFor(expression));
        HtmlContentUtilities.AddAttributes(builder, inputOptions.LabelAttributes);
        var text = inputOptions.LabelText ?? htmlHelper.DisplayNameFor(expression);

        if (inputOptions.RequiredSymbol)
        {
            text += "<i class=\"text-error\">*</i>";
        }

        builder.InnerHtml.AppendHtml($"<span>{text}</span>");

        return builder;
    }

    public static IHtmlContent CreateInput<TModel, TResult>(IHtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TResult>> expression, InputOptions inputOptions)
    {
        var divWrap = new TagBuilder("div");
        HtmlContentUtilities.AddAttributes(divWrap, inputOptions.InputWrapAttributes);
        divWrap.AddCssClass(
            $"input input-bordered focus:border-primary focus-within:border-primary flex items-center gap-2 col-span-{inputOptions.InputClos}");

        var innerHtml = divWrap.InnerHtml;

        CreateInputIcon(innerHtml, inputOptions.IconCls);

        var inputAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(inputOptions.InputAttributes) ??
                              new Dictionary<string, object>();

        if (inputAttributes.ContainsKey("class"))
        {
            inputAttributes["class"] += " w-full";
        }
        else
        {
            inputAttributes.Add("class", "w-full");
        }

        if (inputOptions.Placeholder == "auto")
        {
            inputAttributes.Add("placeholder", htmlHelper.DisplayNameFor(expression));
        }

        inputAttributes.Add("autocomplete", inputOptions.Autocomplete);

        switch (inputOptions.Type)
        {
            case InputType.Text:
                innerHtml.AppendHtml(htmlHelper.TextBoxFor(expression, inputAttributes));
                break;
            case InputType.Password:
                innerHtml.AppendHtml(htmlHelper.PasswordFor(expression, inputAttributes));
                break;
            case InputType.Select:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(inputOptions.Type), inputOptions.Type, null);
        }

        CreateClearButton(innerHtml, inputOptions);

        if (inputOptions.Type == InputType.Password)
        {
            CreateEyeButton(innerHtml);
        }

        return divWrap;
    }

    public static void CreateInputIcon(IHtmlContentBuilder innerHtml, string? iconCls)
    {
        if (iconCls.IsNullOrEmpty())
        {
            return;
        }

        innerHtml.AppendHtml($"<i class=\"{iconCls} w-4 h-4  opacity-70\"></i>");
    }

    public static void CreateClearButton(IHtmlContentBuilder innerHtml, InputOptions inputAttribute)
    {
        if (inputAttribute.Clearable)
        {
            innerHtml.AppendHtml(
                $"<button tabindex=\"-1\" type=\"button\"  class=\"clear-button hidden\" ><i class=\"fas fa-times w-5 h-5 text-base opactity-70 \"></i></button>");
        }
    }

    public static void CreateEyeButton(IHtmlContentBuilder innerHtml)
    {
        innerHtml.AppendHtml(
            "<button tabindex=\"-1\" type=\"button\"  class=\"show-password-button hidden\" ><i class=\"fas fa-eye w-5 h-5 text-base opactity-70\"></i></button>");
    }

    public static void CreatePasswordIndicator(IHtmlContentBuilder innerHtml, InputOptions inputAttribute)
    {
        if (inputAttribute.Autocomplete != "new-password" || !inputAttribute.HasPasswordIndicator)
        {
            return;
        }

        var html =
            $"<div class=\"indicator flex w-full justify-between gap-1 items-center mt-1 {inputAttribute.GetStartCls()} col-span-{inputAttribute.InputClos}\">";
        for (var i = 0; i <= 4; i++)
        {
            html += $"<progress  class=\"w-1/5 progress progress-gray-200\" value=\"0\" max=\"100\"></progress >";
        }

        html += "</div>";
        innerHtml.AppendHtml(html);
    }

    public static void CreateErrorWarp(IHtmlContentBuilder innerHtml, InputOptions inputAttribute)
    {
        var builder = new TagBuilder("div");
        builder.AddCssClass(inputAttribute.GetStartCls());
        builder.AddCssClass($"col-span-{inputAttribute.InputClos}");
        builder.AddCssClass("label hidden");
        HtmlContentUtilities.AddAttributes(builder, inputAttribute.ErrorAttributes);
        builder.InnerHtml.AppendHtml("<span class=\"label-text-alt text-xs text-error \"></span>");
        innerHtml.AppendHtml(builder);
    }
}