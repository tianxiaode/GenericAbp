using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

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
        var (innerHtml,isCheckBox,isTextArea) = await GetFormInputGroupAsHtmlAsync(context, output, formContent);

        if (isCheckBox && TagHelper.CheckBoxHiddenInputRenderMode.HasValue)
        {
            TagHelper.ViewContext.CheckBoxHiddenInputRenderMode = TagHelper.CheckBoxHiddenInputRenderMode.Value;
        }

        var order = TagHelper.AspFor?.ModelExplorer.GetDisplayOrder() ?? 0;

        AddGroupToFormGroupContents(
            context,
            TagHelper.AspFor?.Name,
            SurroundInnerHtmlAndGet(context, output, innerHtml, isCheckBox),
            order,
            out var suppress
        );

        if (suppress)
        {
            output.SuppressOutput();
            return;
        }
        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "div";
        var cols = formContent?.Cols ?? 1;
        if (isTextArea) cols = 1;
        output.Attributes.AddClass($"w-cols-{cols} pt-1 pb-6");

        if (formContent?.Horizontal == true )
        {
            output.Attributes.AddClass("d-flex");
            if (!isTextArea)
            {
                var heightCls = TagHelper.Size switch
                {
                    MetroFormControlSize.Large => "h-input-wrap-large",
                    MetroFormControlSize.Small => "h-input-wrap-small",
                    _ => "h-input-wrap",
                };
                output.Attributes.AddClass(heightCls);
            }
        }

        //if (order > 0)
        //{
        //    output.Attributes.AddStyle(  "order", order.ToString());
        //}
        output.Attributes.RemoveClass("data-validate");
        output.Content.AppendHtml(innerHtml);
    }

    protected virtual async Task<(string,bool, bool)> GetFormInputGroupAsHtmlAsync(TagHelperContext context, TagHelperOutput output, FormContent formContent)
    {
        var (inputTag ,isCheckBox, isTextarea)  = await GetInputTagHelperOutputAsync(context, output, formContent);

        inputTag.Attributes.Add("data-validate", await GetInputValidatorAsync(inputTag.Attributes));
        var inputHtml = await inputTag.RenderAsync(_encoder);
        var label = await GetLabelAsHtmlAsync(context, output, inputTag, formContent, isCheckBox);

        return (GetContent(context, output, label, inputHtml, isCheckBox),isCheckBox, isTextarea);
    }

    protected virtual string GetContent(TagHelperContext context, TagHelperOutput output, string label, string inputHtml, bool isCheckBox)
    {

        //inputHtml = $"<div class='flex-fill {heightCls}'>{inputHtml}</div>";
        var innerContent = label + inputHtml;

        return innerContent ;
    }

    protected virtual async Task<(TagHelperOutput,bool, bool)> GetInputTagHelperOutputAsync(TagHelperContext context, TagHelperOutput output, FormContent formContent)
    {
        var tagHelper = GetInputTagHelper(context, output);

        var inputTagHelperOutput = await tagHelper.ProcessAndGetOutputAsync(
            GetInputAttributes(context, output),
            context,
            "input"
        );

        var isTextArea = ConvertToTextAreaIfTextArea(inputTagHelperOutput);
        AddDisabledAttribute(inputTagHelperOutput);
        var isCheckbox = IsInputCheckbox(context, output, inputTagHelperOutput.Attributes);
        AddReadOnlyAttribute(inputTagHelperOutput);
        await AddPlaceholderAttributeAsync(inputTagHelperOutput);
        if (isCheckbox)
        {
            await SetCheckBoxAttributesAsync(inputTagHelperOutput);
        }else
        {
            inputTagHelperOutput.Attributes.AddClass("metro-input");
        }

        SetDataRoleAttribute(inputTagHelperOutput,isCheckbox, isTextArea);
        return (inputTagHelperOutput,isCheckbox, isTextArea);
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

        if (!TagHelper.Name.IsNullOrEmpty() && !attrList.ContainsName("Name"))
        {
            attrList.Add("Name", TagHelper.Name);
        }

        if (!TagHelper.Value.IsNullOrEmpty() && !attrList.ContainsName("value"))
        {
            attrList.Add("value", TagHelper.Value);
        }
        return attrList;
    }

    protected virtual async Task<string> GetLabelAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag, FormContent formContent, bool isCheckbox)
    {
        if (IsOutputHidden(inputTag) || TagHelper.SuppressLabel)
        {
            return string.Empty;
        }

        if (string.IsNullOrEmpty(TagHelper.Label))
        {
            return await GetLabelAsHtmlUsingTagHelperAsync(context, output, formContent, isCheckbox) ;
        }

        var label = new TagBuilder("label");
        label.Attributes.Add("for", GetIdAttributeValue(inputTag));
        
        if(!isCheckbox)label.InnerHtml.AppendHtml(TagHelper.Label);
        if (formContent.Horizontal)
        {
            label.Attributes.Add("style", $"min-width:{formContent.LabelWidth}px;");
        }
        var lineHeight = TagHelper.Size switch
        {
            MetroFormControlSize.Large => "lh-input-label-large",
            MetroFormControlSize.Small => "lh-input-label-small",
            _ => "lh-input-label",
        };
        label.AddCssClass(lineHeight);


        return label.ToHtmlString();
    }

    protected virtual async Task<string> GetLabelAsHtmlUsingTagHelperAsync(TagHelperContext context, TagHelperOutput output, FormContent formContent, bool isCheckbox)
    {
        var labelTagHelper = new LabelTagHelper(_generator)
        {
            For = TagHelper.AspFor,
            ViewContext = TagHelper.ViewContext
        };
        var attributeList = new TagHelperAttributeList {  };

        if (formContent.Horizontal)
        {
            attributeList.Add("style", $"min-width:{formContent.LabelWidth}px;");
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


        AddDisabledAttribute(labelTagHelperOutput);
        AddReadOnlyAttribute(labelTagHelperOutput);

        if (isCheckbox)
        {
            labelTagHelperOutput.Attributes.AddClass("checkbox-label");
        }
        else
        {
            labelTagHelperOutput.Content.AppendHtml(GetRequiredSymbol(context, output));
            if (formContent.Horizontal)
            {
                labelTagHelperOutput.Content.AppendHtml(": ");
            }
        }
        var labelHtml = await labelTagHelperOutput.RenderAsync(_encoder);
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

    protected virtual async Task AddPlaceholderAttributeAsync(TagHelperOutput inputTagHelperOutput)
    {
        if (inputTagHelperOutput.Attributes.ContainsName("placeholder"))
        {
            return;
        }

        var attribute = TagHelper.AspFor.ModelExplorer.GetAttribute<Placeholder>();

        if (attribute == null) return;
        var placeholderLocalized = await _tagHelperLocalizer.GetLocalizedTextAsync(attribute.Value, TagHelper.AspFor.ModelExplorer);

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
                case "length":
                    validateAttributeValue += $" {name}={attributeList["data-val-length-max"]?.Value}";
                    continue;
                default:
                    validateAttributeValue += $" {name}";
                    break;
            }
        }

        return Task.FromResult(validateAttributeValue);
    }

    protected virtual bool ConvertToTextAreaIfTextArea(TagHelperOutput tagHelperOutput)
    {
        var textAreaAttribute = TryGetTextAreaAttribute(tagHelperOutput);

        if (textAreaAttribute == null)
        {
            return false;
        }

        tagHelperOutput.TagName = "textarea";
        tagHelperOutput.TagMode = TagMode.StartTagAndEndTag;
        tagHelperOutput.Content.SetContent(TagHelper.AspFor.ModelExplorer.Model?.ToString());
        tagHelperOutput.Attributes.AddClass("flex-fill");
        if (textAreaAttribute.Rows > 0)
        {
            tagHelperOutput.Attributes.Add("rows", textAreaAttribute.Rows);
        }
        if (textAreaAttribute.Cols > 0)
        {
            tagHelperOutput.Attributes.Add("cols", textAreaAttribute.Cols);
        }

        return true;
    }

    protected virtual TextArea TryGetTextAreaAttribute(TagHelperOutput output)
    {
        var textAreaAttribute = TagHelper.AspFor.ModelExplorer.GetAttribute<TextArea>();

        if (textAreaAttribute == null && output.Attributes.Any(a => a.Name == "text-area"))
        {
            return new TextArea();
        }

        return textAreaAttribute;
    }

    protected virtual bool IsInputCheckbox(TagHelperContext context, TagHelperOutput output, TagHelperAttributeList attributes)
    {
        return attributes.Any(a => a.Value != null && a.Name == "type" && a.Value.ToString() == "checkbox");
    }

    protected virtual void SetDataRoleAttribute(TagHelperOutput output, bool isCheckbox, bool isTextarea)
    {
        var dataRole = "input";
        if (isCheckbox) dataRole = "checkbox";
        if (isTextarea) dataRole = "textarea";
        output.Attributes.Add("data-role", dataRole);

    }

    protected virtual async Task SetCheckBoxAttributesAsync(TagHelperOutput output)
    {
        var name = TagHelper.Label ?? TagHelper.AspFor.Name;
        name = name[(name?.IndexOf(".") + 1 ?? 1)..];
        var label = await _tagHelperLocalizer.GetLocalizedTextAsync(name, TagHelper.AspFor.ModelExplorer);
        output.Attributes.Add("data-caption", label);
        output.Attributes.Add("data-style", 2);
        output.Attributes.Add("data-cls-caption", "fg-cyan");
        output.Attributes.Add("data-cls-check", "bd-cyan");
    }

    protected virtual string GetIdAttributeValue(TagHelperOutput inputTag)
    {
        var idAttr = inputTag.Attributes.FirstOrDefault(a => a.Name == "id");

        return idAttr != null ? idAttr.Value.ToString() : string.Empty;
    }

    protected virtual void AddGroupToFormGroupContents(TagHelperContext context, string propertyName, string html, int order, out bool suppress)
    {
        var list = context.GetValue<List<FormItem>>(FormItems);
        suppress = list == null;

        if (list != null && !list.Any(igc => igc.HtmlContent.Contains("id=\"" + propertyName.Replace('.', '_') + "\"")))
        {
            list.Add(new FormItem
            {
                HtmlContent = html,
                Order = order,
                PropertyName = propertyName
            });
        }
    }
    protected virtual string SurroundInnerHtmlAndGet(TagHelperContext context, TagHelperOutput output, string innerHtml, bool isCheckbox)
    {
        return "<div class=\"" + (isCheckbox ? "custom-checkbox custom-control mb-2 form-check" : "mb-3") + "\">" +
               Environment.NewLine + innerHtml + Environment.NewLine +
               "</div>";
    }

}
