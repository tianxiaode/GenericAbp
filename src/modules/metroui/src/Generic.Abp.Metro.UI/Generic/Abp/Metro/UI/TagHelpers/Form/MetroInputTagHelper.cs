using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroInputTagHelper : MetroInputTagHelperBase
{
    public MetroInputTagHelper(HtmlEncoder htmlEncoder, IMetroTagHelperLocalizerService localizer,
        IHtmlGenerator generator) : base(htmlEncoder, generator, localizer)
    {
    }

    protected override async Task<string> GetFormInputGroupAsHtmlAsync(TagHelperContext context, TagHelperOutput output)
    {
        var inputTag = await GetInputTagHelperOutputAsync(context, output);
        if (IsOutputHidden(inputTag))
        {
            IsHidden = true;
        }

        var inputHtml = await inputTag.RenderAsync(HtmlEncoder);
        var label = NoLabel ? "" : await GetLabelAsHtmlAsync(inputTag);

        return GetContent(label, inputHtml);
    }

    protected virtual string GetContent(string label, string inputHtml)
    {
        var innerContent = label + inputHtml;

        return innerContent;
    }

    protected virtual async Task<TagHelperOutput> GetInputTagHelperOutputAsync(TagHelperContext context,
        TagHelperOutput output)
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
        await AddPlaceholderAttributeAsync(inputTagHelperOutput);
        if (IsCheckbox)
        {
            await SetCheckBoxAttributesAsync(inputTagHelperOutput);
        }
        else
        {
            inputTagHelperOutput.Attributes.AddClass("metro-input");
            await AddPrependAndAppendAttributesAsync(inputTagHelperOutput);
        }

        await SetDataRoleAttributeAsync(inputTagHelperOutput);
        await SetInputValidatorAsync(inputTagHelperOutput.Attributes);

        await SetInputSizeAsync(inputTagHelperOutput);

        return inputTagHelperOutput;
    }

    protected virtual TagHelper GetInputTagHelper(TagHelperContext context, TagHelperOutput output)
    {
        if (TryGetTextAreaAttribute(output) != null)
        {
            var textAreaTagHelper = new TextAreaTagHelper(Generator)
            {
                For = AspFor,
                ViewContext = ViewContext
            };

            if (!string.IsNullOrWhiteSpace(Name))
            {
                textAreaTagHelper.Name = Name;
            }

            return textAreaTagHelper;
        }

        var inputTagHelper = new InputTagHelper(Generator)
        {
            For = AspFor,
            InputTypeName = InputTypeName,
            ViewContext = ViewContext
        };

        if (!string.IsNullOrWhiteSpace(Format))
        {
            inputTagHelper.Format = Format;
        }

        if (!string.IsNullOrWhiteSpace(Name))
        {
            inputTagHelper.Name = Name;
        }

        if (!string.IsNullOrWhiteSpace(Value))
        {
            inputTagHelper.Value = Value;
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

        if (!string.IsNullOrWhiteSpace(InputTypeName) && !attrList.ContainsName("type"))
        {
            attrList.Add("type", InputTypeName);
        }

        if (!string.IsNullOrWhiteSpace(Name) && !attrList.ContainsName("Name"))
        {
            attrList.Add("name", Name);
        }

        if (!string.IsNullOrWhiteSpace(Value) && !attrList.ContainsName("value"))
        {
            attrList.Add("value", Value);
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
        tagHelperOutput.Content.SetContent(AspFor.ModelExplorer.Model?.ToString());
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
        var textAreaAttribute = AspFor.ModelExplorer.GetAttribute<TextArea>();

        if (textAreaAttribute == null && output.Attributes.Any(a => a.Name == "text-area"))
        {
            return new TextArea();
        }

        return textAreaAttribute;
    }

    protected virtual Task<bool> IsInputCheckboxAsync(TagHelperContext context, TagHelperOutput output,
        TagHelperAttributeList attributes)
    {
        return Task.FromResult(attributes.Any(a =>
            a.Value != null && a.Name == "type" && a.Value.ToString() == "checkbox"));
    }

    protected virtual async Task SetCheckBoxAttributesAsync(TagHelperOutput output)
    {
        output.Attributes.Add("data-caption", await GetLabelDisplayNameAsync());
        output.Attributes.Add("data-style", 2);
    }
}