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
        InputAttribute inputAttribute)
    {
        ArgumentNullException.ThrowIfNull(htmlHelper);
        ArgumentNullException.ThrowIfNull(expression);

        var divBuilder = new TagBuilder("div");
        divBuilder.AddCssClass("grid grid-cols-12 w-full gap-1 mb-4");

        var innerHtml = divBuilder.InnerHtml;
        HtmlContentUtilities.AddAttributes(divBuilder, inputAttribute.FormControlAttributes);


        if (inputAttribute.LabelCols > 0)
        {
            innerHtml.AppendHtml(CreateLabel(htmlHelper, expression, inputAttribute.LabelCols,
                inputAttribute.LabelAttributes));
        }

        innerHtml.AppendHtml(CreateInput(htmlHelper, expression, inputAttribute));

        if (inputAttribute.Type == InputType.Password)
        {
            CreatePasswordIndicator(innerHtml, inputAttribute);
        }


        return divBuilder;
        ;
    }

    public static IHtmlContent CreateLabel<TModel, TResult>(IHtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TResult>> expression, int labelCols,
        object? labelAttributes = null)
    {
        var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(labelAttributes) ??
                         new Dictionary<string, object>();
        var classname = $"label col-span-{labelCols} ";
        if (attributes.ContainsKey("classname"))
        {
            classname += attributes["classname"];
            attributes.Remove("classname");
        }

        attributes.Add("class", classname);
        return htmlHelper.LabelFor(expression, attributes);
    }

    public static IHtmlContent CreateInput<TModel, TResult>(IHtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TResult>> expression, InputAttribute inputAttribute)
    {
        var divWrap = new TagBuilder("div");
        divWrap.AddCssClass(
            $"input input-bordered focus:border-primary focus-within:border-primary flex items-center gap-2 col-span-{inputAttribute.InputClos}");

        var innerHtml = divWrap.InnerHtml;

        CreateInputIcon(innerHtml, inputAttribute.IconCls);

        var inputAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(inputAttribute.InputAttributes) ??
                              new Dictionary<string, object>();
        if (inputAttribute.Placeholder == "auto")
        {
            inputAttributes.Add("placeholder", htmlHelper.DisplayNameFor(expression));
        }

        switch (inputAttribute.Type)
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
                throw new ArgumentOutOfRangeException(nameof(inputAttribute.Type), inputAttribute.Type, null);
        }

        CreateClearButton(innerHtml, inputAttribute);

        if (inputAttribute.Type == InputType.Password)
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

    public static void CreateClearButton(IHtmlContentBuilder innerHtml, InputAttribute inputAttribute)
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

    public static void CreatePasswordIndicator(IHtmlContentBuilder innerHtml, InputAttribute inputAttribute)
    {
        if (inputAttribute.Autocomplete != "new-password")
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

    public static void CreateErrorWarp(IHtmlContentBuilder innerHtml, InputAttribute inputAttribute)
    {
        var builder = new TagBuilder("div");
        builder.AddCssClass(inputAttribute.GetStartCls());
        builder.AddCssClass($"col-span-{inputAttribute.InputClos}");
        builder.AddCssClass("label hidden");
        HtmlContentUtilities.AddAttributes(builder, inputAttribute.ErrorAttributes);
        builder.InnerHtml.AppendHtml("<span class=\"flabel-text-alt text-xs text-error\"></span>");
    }
}