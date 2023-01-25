using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroInputTagHelperService : MetroInputTagHelperServiceBase<MetroInputTagHelper>
{
    protected IMetroTagHelperLocalizer TagHelperLocalizer { get; }
    public MetroInputTagHelperService(IHtmlGenerator generator, HtmlEncoder encoder, IMetroTagHelperLocalizer tagHelperLocalizer) : base(generator,encoder)
    {
        TagHelperLocalizer = tagHelperLocalizer;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await GetFormContentAsync(context);
        await SetOrderAsync(output);
        var innerHtml = await GetFormInputGroupAsHtmlAsync(context, output);

        if (IsCheckbox && TagHelper.CheckBoxHiddenInputRenderMode.HasValue)
        {
            TagHelper.ViewContext.CheckBoxHiddenInputRenderMode = TagHelper.CheckBoxHiddenInputRenderMode.Value;
        }

        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "div";

        await SetInputSizeAsync(output);
        output.Content.AppendHtml(innerHtml);
        var suppress =await AddItemToFromItemsAsync(context, FormItems, TagHelper.AspFor.Name, Order, await output.RenderAsync(Encoder));
        
        if (suppress)
        {
            
            output.SuppressOutput();
        }
    }

    protected virtual async Task<string> GetFormInputGroupAsHtmlAsync(TagHelperContext context, TagHelperOutput output)
    {
        var inputTag  = await GetInputTagHelperOutputAsync(context, output);

        var inputHtml = await inputTag.RenderAsync(Encoder);
        var label = await GetLabelAsHtmlAsync(inputTag, TagHelperLocalizer);

        return GetContent(label, inputHtml);
    }

    protected virtual string GetContent(string label, string inputHtml)
    {

        var innerContent = label + inputHtml;

        return innerContent ;
    }

    protected virtual async Task<TagHelperOutput> GetInputTagHelperOutputAsync(TagHelperContext context, TagHelperOutput output)
    {
        var tagHelper = GetInputTagHelper(context, output);

        var inputTagHelperOutput = await tagHelper.ProcessAndGetOutputAsync(
            GetInputAttributes(context, output),
            context,
            "input"
        );

        IsTextarea = ConvertToTextAreaIfTextArea(inputTagHelperOutput);
        AddDisabledAttribute(inputTagHelperOutput);
        IsCheckbox = await IsInputCheckboxAsync(context, output, inputTagHelperOutput.Attributes);
        AddReadOnlyAttribute(inputTagHelperOutput);
        await AddPlaceholderAttributeAsync(inputTagHelperOutput, TagHelperLocalizer);
        if (IsCheckbox)
        {
            await SetCheckBoxAttributesAsync(inputTagHelperOutput, TagHelperLocalizer);
        }else
        {
            inputTagHelperOutput.Attributes.AddClass("metro-input");
        }

        await SetDataRoleAttributeAsync(inputTagHelperOutput);
        await SetInputValidatorAsync(inputTagHelperOutput.Attributes);
        return inputTagHelperOutput;
    }

    protected virtual TagHelper GetInputTagHelper(TagHelperContext context, TagHelperOutput output)
    {

        var inputTagHelper = new InputTagHelper(Generator)
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
            attrList.Add("name", TagHelper.Name);
        }

        if (!TagHelper.Value.IsNullOrEmpty() && !attrList.ContainsName("value"))
        {
            attrList.Add("value", TagHelper.Value);
        }
        return attrList;
    }


    protected virtual bool ConvertToTextAreaIfTextArea(TagHelperOutput tagHelperOutput)
    {
        var textAreaAttribute = TryGetTextAreaAttribute(tagHelperOutput);

        if (textAreaAttribute == null)
        {
            return false;
        }

        IsTextarea = true;
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
    protected virtual Task<bool> IsInputCheckboxAsync(TagHelperContext context, TagHelperOutput output, TagHelperAttributeList attributes)
    {
        return Task.FromResult(attributes.Any(a => a.Value != null && a.Name == "type" && a.Value.ToString() == "checkbox"));
    }

    protected virtual async Task SetCheckBoxAttributesAsync(TagHelperOutput output, IMetroTagHelperLocalizer tagHelperLocalizer)
    {
        output.Attributes.Add("data-caption", await GetLabelDisplayNameAsync(tagHelperLocalizer));
        output.Attributes.Add("data-style", 2);
        //output.Attributes.Add("data-cls-caption", "fg-cyan");
        //output.Attributes.Add("data-cls-check", "bd-cyan");
    }

}
