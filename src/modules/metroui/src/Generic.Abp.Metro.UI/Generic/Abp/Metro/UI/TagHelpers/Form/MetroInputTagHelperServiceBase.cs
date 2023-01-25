using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

//public abstract class MetroTagHelperService<TTagHelper> : IMetroTagHelperService<TTagHelper>
//    where TTagHelper : TagHelper

public abstract class MetroInputTagHelperServiceBase<TTagHelper> :MetroTagHelperService<TTagHelper>
where TTagHelper : TagHelper,IMetroInputTagHelperBase
{
    protected MetroInputTagHelperServiceBase(IHtmlGenerator generator, HtmlEncoder encoder)
    {
        Generator = generator;
        Encoder = encoder;
    }

    protected IHtmlGenerator Generator { get; }
    protected HtmlEncoder Encoder { get; }
    protected FormContent FormContent { get; set; }
    protected bool IsTextarea { get; set ;} = false;
    protected bool IsCheckbox { get; set; } = false;
    protected bool IsCheckboxGroup { get; set; } = false;
    protected bool IsRadio { get; set; } = false;
    protected bool IsSelect { get; set; } = false;
    protected string Label { get; set; }

    protected Task GetFormContentAsync(TagHelperContext context)
    {
        FormContent = context.Items["FormContent"] as FormContent;
        return Task.CompletedTask;
    }

    protected Task SetInputSizeAsync(TagHelperOutput output)
    {
        var attributes = output.Attributes;
        var cols = FormContent?.Cols ?? 1;
        if (IsTextarea || IsCheckbox || IsRadio || IsCheckboxGroup) cols = 1;
        attributes.AddClass($"w-cols-{cols} pt-1 pb-6");
        if (FormContent is not { Horizontal: true }) return Task.CompletedTask;
        attributes.AddClass("d-flex");
        return Task.CompletedTask;
        //var size = await GetSizeStringAsync();
        //if (!size.IsNullOrWhiteSpace())
        //{
        //    size = "-" + size;
        //}
        //attributes.AddClass($"h-input-wrap{size}");
    }

    protected async Task SetLabelSizeAsync(TagBuilder tagBuilder)
    {
        var size = await GetSizeStringAsync();
        //line-height
        tagBuilder.AddCssClass($"lh-input-label{size}");

        if (FormContent is not { Horizontal: true }) return;

        //label width
        tagBuilder.AddCssClass($"w-label-{(int)FormContent.LabelWidth}");
    }

    protected Task<string> GetSizeStringAsync()
    {
        var size = TagHelper.Size switch
        {
            MetroFormControlSize.Large => "large",
            MetroFormControlSize.Small => "small",
            _ => "",
        };
        if (!size.IsNullOrWhiteSpace()) size = "-" + size;
        return Task.FromResult(size);
    }



    protected virtual void AddDisabledAttribute(TagHelperOutput inputTagHelperOutput)
    {
        if (inputTagHelperOutput.Attributes.ContainsName("disabled") == false &&
            (TagHelper.IsDisabled || TagHelper.AspFor.ModelExplorer.GetAttribute<DisabledInput>() != null))
        {
            inputTagHelperOutput.Attributes.Add("disabled", "");
        }
    }

    protected virtual void AddReadOnlyAttribute(TagHelperOutput inputTagHelperOutput)
    {
        if (inputTagHelperOutput.Attributes.ContainsName("readonly") == false &&
            (TagHelper.IsReadonly != false || TagHelper.AspFor.ModelExplorer.GetAttribute<ReadOnlyInput>() != null))
        {
            inputTagHelperOutput.Attributes.Add("readonly", "");
        }
    }

    protected virtual Task SetDataRoleAttributeAsync(TagHelperOutput output)
    {
        var type = output.Attributes["type"]?.Value?.ToString();
        var dataRole = type switch
        {
            "date" => "calendarpicker",
            "datetime-local" => "calendarpicker",
            "file" => "file",
            _ => "input"
        };
        if (type == "datetime-local")
        {
            output.Attributes.Add("data-show-time", "true");
        }

        if (dataRole == "calendarpicker") {
            output.Attributes.Add("data-format", CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern.Replace("y","Y"));
        }
        if (IsCheckbox) dataRole = "checkbox";
        if (IsTextarea) dataRole = "textarea";
        if (IsSelect) dataRole = "select";
        output.Attributes.Add("data-role", dataRole);
        return Task.CompletedTask;
    }

    protected virtual async Task<string> GetLabelAsHtmlAsync(TagHelperOutput inputTag, IMetroTagHelperLocalizer tagHelperLocalizer)
    {
        if (IsOutputHidden(inputTag) || IsCheckbox)
        {
            return string.Empty;
        }

        var resolvedLabelText = await GetLabelDisplayNameAsync(tagHelperLocalizer);

        var label = new TagBuilder("label");
    
        label.InnerHtml.AppendHtml(await tagHelperLocalizer.GetLocalizedTextAsync(resolvedLabelText, TagHelper.AspFor.ModelExplorer));
        await SetLabelSizeAsync(label);
        if (inputTag.Attributes.ContainsName("disabled"))
        {
            label.AddCssClass("disabled");
        }
        if (inputTag.Attributes.ContainsName("readonly"))
        {
            label.AddCssClass("readonly");
        }
        if (IsCheckbox)
        {
            label.AddCssClass("checkbox-label");
            label.AddCssClass("metro-input");
        }
        else
        {
            label.InnerHtml.AppendHtml(GetRequiredSymbol());
            if (FormContent.Horizontal)
            {
                label.InnerHtml.AppendHtml(": ");
            }
        }

        return label.ToHtmlString();
    }

    protected virtual async Task<string> GetLabelDisplayNameAsync(IMetroTagHelperLocalizer tagHelperLocalizer)
    {
        if (!string.IsNullOrWhiteSpace(Label)) return Label;

        var resolvedLabelText = TagHelper.Label ??
                                TagHelper.AspFor.Metadata.DisplayName ??
                                TagHelper.AspFor.Metadata.PropertyName;
        var expression = TagHelper.AspFor.Name;
        if (resolvedLabelText == null && expression != null)
        {
            var index = expression.LastIndexOf('.');
            // Expression does not contain a dot separator.
            resolvedLabelText = index == -1 ? expression : expression[(index + 1)..];
        }

        if (string.IsNullOrEmpty(TagHelper.Label))
        {
            TagHelper.Label = TagHelper.Name ?? TagHelper.AspFor.Name;
            //return await GetLabelAsHtmlUsingTagHelperAsync(context, output) ;
        }

        Label = await tagHelperLocalizer.GetLocalizedTextAsync(resolvedLabelText, TagHelper.AspFor.ModelExplorer);
        return Label;
    }

    protected virtual bool IsOutputHidden(TagHelperOutput inputTag)
    {
        return inputTag.Attributes.Any(a => a.Name.ToLowerInvariant() == "type" && a.Value.ToString()?.ToLowerInvariant() == "hidden");
    }

    protected virtual string GetRequiredSymbol()
    {
        if (!TagHelper.DisplayRequiredSymbol)
        {
            return "";
        }

        if (!FormContent.RequiredSymbols)
        {
            return "";
        }

        return TagHelper.AspFor.ModelExplorer.GetAttribute<RequiredAttribute>() != null ? "<span class='fg-red'> * </span>" : "";
    }



    protected virtual Task SetInputValidatorAsync(TagHelperAttributeList attributes)
    {
        var validateAttributes = attributes.Where(m => m.Name?.StartsWith("data-val-") ?? false);
        var validateAttributeValue = "";
        foreach (var attribute in validateAttributes)
        {
            var name = attribute.Name?.Replace("data-val-","");
            if (name.IsNullOrWhiteSpace() || name?.IndexOf("-") > 0) continue;
            switch (name)
            {
                case "maxlength":
                    validateAttributeValue += $" {name}={attributes["data-val-maxlength-max"]?.Value}";
                    continue;
                case "minlength":
                    validateAttributeValue += $" {name}={attributes["data-val-minlength-min"]?.Value}";
                    continue;
                case "range":
                    validateAttributeValue += $" max={attributes["data-val-range-max"]?.Value}";
                    validateAttributeValue += $" min={attributes["data-val-range-min"]?.Value}";
                    continue;
                case "length":
                    validateAttributeValue += $" {name}={attributes["data-val-length-max"]?.Value}";
                    continue;
                default:
                    validateAttributeValue += $" {name}";
                    break;
            }
        }

        attributes.Add("data-validate", validateAttributeValue);
        return Task.CompletedTask;
    }

    protected virtual string GetIdAttributeValue(TagHelperOutput inputTag)
    {
        var idAttr = inputTag.Attributes.FirstOrDefault(a => a.Name == "id");

        return idAttr != null ? idAttr.Value.ToString() : string.Empty;
    }

    protected virtual async Task AddPlaceholderAttributeAsync(TagHelperOutput inputTagHelperOutput, IMetroTagHelperLocalizer tagHelperLocalizer)
    {
        if (inputTagHelperOutput.Attributes.ContainsName("placeholder"))
        {
            return;
        }

        var attribute = TagHelper.AspFor.ModelExplorer.GetAttribute<Placeholder>();

        if (attribute == null) return;
        var placeholderLocalized = await tagHelperLocalizer.GetLocalizedTextAsync(attribute.Value, TagHelper.AspFor.ModelExplorer);

        inputTagHelperOutput.Attributes.Add("placeholder", placeholderLocalized);
    }

    protected virtual Task SetOrderAsync(TagHelperOutput output)
    {
        var order = TagHelper.Order;
        if (order == 0)
        {
            order = TagHelper.AspFor?.ModelExplorer.GetDisplayOrder() ?? 0;
        }
        Order = order;
        output.Attributes.Add("style",$"order:{order}");
        return Task.CompletedTask;
    }

    protected virtual Task<bool> AddItemToFromItemsAsync(TagHelperContext context, string key, string name, int order , string htmlContent)
    {
        if(string.IsNullOrWhiteSpace(name)) return Task.FromResult(true);
        var list = context.GetValue<List<FormItem>>(key);
        if (list == null ) return Task.FromResult(true);
        var item = new FormItem()
        {
            Name = name,
            Order = order,
            HtmlContent = htmlContent
        };
        list.Add(item);
        return Task.FromResult(false);
    }

}
