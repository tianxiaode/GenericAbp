using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using Nito.AsyncEx;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroInputTagHelperService : MetroTagHelperService<MetroInputTagHelper>
{
    private readonly IHtmlGenerator _generator;
    private readonly HtmlEncoder _encoder;
    private readonly IMetroTagHelperLocalizer _tagHelperLocalizer;
    private const string ValidationForAttributeName = "asp-validation-for";
    protected ILogger<MetroInputTagHelperService> Logger { get; }
    public MetroInputTagHelperService(IHtmlGenerator generator, HtmlEncoder encoder, IMetroTagHelperLocalizer tagHelperLocalizer, ILogger<MetroInputTagHelperService> logger)
    {
        _generator = generator;
        _encoder = encoder;
        _tagHelperLocalizer = tagHelperLocalizer;
        Logger = logger;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var formContent = context.Items["FormContent"] as FormContent;
        var innerHtml = await GetFormInputGroupAsHtmlAsync(context, output, formContent);

        var order = TagHelper.AspFor?.ModelExplorer.GetDisplayOrder() ?? 0;

        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "div";
        output.Attributes.AddClass($"w-cols-{formContent?.Cols ?? 1}");

        if (formContent?.Horizontal == true)
        {
            output.Attributes.AddClass("d-flex py-1");
        }

        if (order > 0)
        {
            output.Attributes.AddStyle(  "order", order.ToString());
        }
        output.Attributes.RemoveClass("data-validate");
        output.Attributes.Add("data-role", "inputextension");
        output.Content.AppendHtml(innerHtml);
    }

    protected virtual async Task<string> GetFormInputGroupAsHtmlAsync(TagHelperContext context, TagHelperOutput output, FormContent formContent)
    {
        var inputTag  = await GetInputTagHelperOutputAsync(context, output, formContent);
        inputTag.Attributes.Add("data-validate", await GetInputValidatorAsync(inputTag.Attributes));
        var inputHtml = inputTag.Render(_encoder);
        var label = await GetLabelAsHtmlAsync(context, output, inputTag, formContent);

        return GetContent(context, output, label, inputHtml);
    }

    protected virtual string GetContent(TagHelperContext context, TagHelperOutput output, string label, string inputHtml)
    {
        var heightCls = TagHelper.Size switch
        {
            MetroFormControlSize.Large => "h-input-wrap-large",
            MetroFormControlSize.Small => "h-input-wrap-small",
            _ => "h-input-wrap",
        };

        inputHtml = $"<div class='flex-fill {heightCls}'>{inputHtml}<span class='invalid_feedback'></span></div>";
        var innerContent = label + inputHtml;

        return innerContent ;
    }

    protected virtual async Task<TagHelperOutput> GetInputTagHelperOutputAsync(TagHelperContext context, TagHelperOutput output, FormContent formContent)
    {
        var tagHelper = GetInputTagHelper(context, output);

        var inputTagHelperOutput = await tagHelper.ProcessAndGetOutputAsync(
            GetInputAttributes(context, output),
            context,
            "input"
        );

        AddDisabledAttribute(inputTagHelperOutput);
        AddReadOnlyAttribute(inputTagHelperOutput);
        AddPlaceholderAttribute(inputTagHelperOutput);
        //inputTagHelperOutput.Attributes.Add("data-role", "input");
        return inputTagHelperOutput;
    }

    protected virtual TagHelper GetInputTagHelper(TagHelperContext context, TagHelperOutput output)
    {

        var inputTagHelper = new InputTagHelper(_generator)
        {
            For = TagHelper.AspFor,
            InputTypeName = TagHelper.InputTypeName,
            ViewContext = TagHelper.ViewContext
        };

        if (!TagHelper.Format.IsNullOrEmpty())
        {
            inputTagHelper.Format = TagHelper.Format;
        }

        if (!TagHelper.Name.IsNullOrEmpty())
        {
            inputTagHelper.Name = TagHelper.Name;
        }

        if (!TagHelper.Value.IsNullOrEmpty())
        {
            inputTagHelper.Value = TagHelper.Value;
        }


        return inputTagHelper;
    }


    protected virtual TagHelperAttributeList GetInputAttributes(TagHelperContext context, TagHelperOutput output)
    {
        var tagHelperAttributes = output.Attributes;

        var attrList = new TagHelperAttributeList();

        foreach (var tagHelperAttribute in tagHelperAttributes)
        {
            attrList.Add(tagHelperAttribute);
        }

        if (!TagHelper.InputTypeName.IsNullOrEmpty() && !attrList.ContainsName("type"))
        {
            attrList.Add("type", TagHelper.InputTypeName);
        }

        if (!TagHelper.Name.IsNullOrEmpty() && !attrList.ContainsName("name"))
        {
            attrList.Add("name", TagHelper.Name);
        }

        if (!TagHelper.Value.IsNullOrEmpty() && !attrList.ContainsName("value"))
        {
            attrList.Add("value", TagHelper.Value);
        }
        attrList.Add("class", "metro-input");
        return attrList;
    }

    protected virtual async Task<string> GetLabelAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag, FormContent formContent)
    {
        if (IsOutputHidden(inputTag) || TagHelper.SuppressLabel)
        {
            return string.Empty;
        }

        if (string.IsNullOrEmpty(TagHelper.Label))
        {
            return await GetLabelAsHtmlUsingTagHelperAsync(context, output, formContent) ;
        }

        var label = new TagBuilder("label");
        label.InnerHtml.AppendHtml(TagHelper.Label);


        return label.ToHtmlString();
    }

    protected virtual async Task<string> GetLabelAsHtmlUsingTagHelperAsync(TagHelperContext context, TagHelperOutput output, FormContent formContent)
    {
        var labelTagHelper = new LabelTagHelper(_generator)
        {
            For = TagHelper.AspFor,
            ViewContext = TagHelper.ViewContext
        };
        var attributeList = new TagHelperAttributeList {  };

        if (formContent.Horizontal)
        {
            attributeList.Add("style", $"width:{formContent.LabelWidth}px;");
        }

        var lineHeight = TagHelper.Size switch
        {
            MetroFormControlSize.Large => "lh-input-label-large",
            MetroFormControlSize.Small => "lh-input-label-small",
            _ => "lh-input-label",
        };
        attributeList.AddClass(lineHeight);
        var labelTagHelperOutput = await labelTagHelper.ProcessAndGetOutputAsync(
            attributeList,
            context,
            "label",
            TagMode.StartTagAndEndTag
        );

        if (formContent.Horizontal)
        {
            labelTagHelperOutput.Content.AppendHtml(": ");
        }

        labelTagHelperOutput.Content.AppendHtml(GetRequiredSymbol(context, output));
        AddDisabledAttribute(labelTagHelperOutput);
        AddReadOnlyAttribute(labelTagHelperOutput);
        var labelHtml = labelTagHelperOutput.Render(_encoder);

        return labelHtml;
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

    protected virtual void AddPlaceholderAttribute(TagHelperOutput inputTagHelperOutput)
    {
        if (inputTagHelperOutput.Attributes.ContainsName("placeholder"))
        {
            return;
        }

        var attribute = TagHelper.AspFor.ModelExplorer.GetAttribute<Placeholder>();

        if (attribute == null) return;
        var placeholderLocalized = _tagHelperLocalizer.GetLocalizedText(attribute.Value, TagHelper.AspFor.ModelExplorer);

        inputTagHelperOutput.Attributes.Add("placeholder", placeholderLocalized);
    }

    protected virtual bool IsOutputHidden(TagHelperOutput inputTag)
    {
        return inputTag.Attributes.Any(a => a.Name.ToLowerInvariant() == "type" && a.Value.ToString()?.ToLowerInvariant() == "hidden");
    }

    protected virtual string GetRequiredSymbol(TagHelperContext context, TagHelperOutput output)
    {
        if (!TagHelper.DisplayRequiredSymbol)
        {
            return "";
        }

        return TagHelper.AspFor.ModelExplorer.GetAttribute<RequiredAttribute>() != null ? "<span class='fg-red'> * </span>" : "";
    }

    protected virtual Task<string> GetInputValidatorAsync(TagHelperAttributeList attributeList)
    {
        var validateAttributes = attributeList.Where(m => m.Name?.StartsWith("data-val-") ?? false);
        var validateAttributeValue = "";
        foreach (var attribute in validateAttributes)
        {
            var name = attribute.Name?.Replace("data-val-","");
            if (name.IsNullOrWhiteSpace() || name?.IndexOf("-") > 0) continue;
            switch (name)
            {
                case "maxlength":
                    validateAttributeValue += $" {name}={attributeList["data-val-maxlength-max"]?.Value}";
                    continue;
                case "minlength":
                    validateAttributeValue += $" {name}={attributeList["data-val-minlength-min"]?.Value}";
                    continue;
                case "range":
                    validateAttributeValue += $" max={attributeList["data-val-range-max"]?.Value}";
                    validateAttributeValue += $" min={attributeList["data-val-range-min"]?.Value}";
                    continue;
                default:
                    validateAttributeValue += $" {name}";
                    break;
            }
        }

        return Task.FromResult(validateAttributeValue);
    }

}
