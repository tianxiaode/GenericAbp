using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

[HtmlTargetElement(TagStructure = TagStructure.WithoutEndTag)]
public abstract class MetroInputTagHelperBase : MetroTagHelper<FormGroupItem>
{
    protected MetroInputTagHelperBase(
        HtmlEncoder htmlEncoder,
        IHtmlGenerator generator,
        IMetroTagHelperLocalizerService localizer
    ) : base(htmlEncoder)
    {
        Localizer = localizer;
        Generator = generator;
        GroupItemsName = TagHelperConsts.FormGroupItems;
    }

    protected IMetroTagHelperLocalizerService Localizer { get; }
    protected IHtmlGenerator Generator { get; }
    protected FormContent FormContent { get; set; }
    protected bool IsTextarea { get; set; } = false;
    protected bool IsCheckbox { get; set; } = false;
    protected bool IsCheckboxGroup { get; set; } = false;
    protected bool IsRadio { get; set; } = false;
    protected bool IsSelect { get; set; } = false;
    protected bool IsRadioGroup { get; set; } = false;
    public ModelExpression AspFor { get; set; }
    [HtmlAttributeName("disabled")] public bool IsDisabled { get; set; } = false;
    [HtmlAttributeName("readonly")] public bool IsReadonly { get; set; } = false;
    public string Label { get; set; }
    [HtmlAttributeName("type")] public string InputTypeName { get; set; }
    [HtmlAttributeName("required-symbol")] public bool DisplayRequiredSymbol { get; set; } = true;
    [HtmlAttributeName("asp-format")] public string Format { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public bool NoLabel { get; set; } = false;
    public int? LabelWidth { get; set; }

    public CheckBoxHiddenInputRenderMode? CheckBoxHiddenInputRenderMode { get; set; }

    public MetroFormControlSize Size { get; set; } = MetroFormControlSize.Default;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await GetFormContentAsync(context);
        var innerHtml = await GetFormInputGroupAsHtmlAsync(context, output);

        if (IsCheckbox && CheckBoxHiddenInputRenderMode.HasValue)
        {
            ViewContext.CheckBoxHiddenInputRenderMode = CheckBoxHiddenInputRenderMode.Value;
        }

        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "div";

        await SetInputSizeAsync(output);
        output.Content.AppendHtml(innerHtml);
        var suppress = await AddItemToFormItemsContents(context, AspFor.Name,
            await output.RenderAsync(HtmlEncoder), AspFor.ModelExplorer.GetDisplayOrder());

        if (suppress)
        {
            output.SuppressOutput();
        }
    }

    protected virtual Task<string> GetFormInputGroupAsHtmlAsync(TagHelperContext context, TagHelperOutput output)
    {
        throw new NotImplementedException();
    }

    protected Task GetFormContentAsync(TagHelperContext context)
    {
        if (!context.Items.ContainsKey(TagHelperConsts.FormContent))
        {
            FormContent = new FormContent();
            return Task.CompletedTask;
        }

        FormContent = context.Items[TagHelperConsts.FormContent] as FormContent ?? new FormContent();
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
    }

    protected async Task SetLabelSizeAsync(TagBuilder tagBuilder)
    {
        var size = await GetSizeStringAsync();
        //line-height
        tagBuilder.AddCssClass($"lh-input-label{size}");
    }

    protected Task<string> GetSizeStringAsync()
    {
        var size = Size switch
        {
            MetroFormControlSize.Large => "large",
            MetroFormControlSize.Small => "small",
            _ => "",
        };
        if (!string.IsNullOrWhiteSpace(size)) size = "-" + size;
        return Task.FromResult(size);
    }


    protected virtual void AddDisabledAttribute(TagHelperOutput inputTagHelperOutput)
    {
        if (inputTagHelperOutput.Attributes.ContainsName("disabled") == false &&
            (IsDisabled || AspFor.ModelExplorer.GetAttribute<DisabledInput>() != null))
        {
            inputTagHelperOutput.Attributes.Add("disabled", "");
        }
    }

    protected virtual void AddReadOnlyAttribute(TagHelperOutput inputTagHelperOutput)
    {
        if (inputTagHelperOutput.Attributes.ContainsName("readonly") == false &&
            (IsReadonly || AspFor.ModelExplorer.GetAttribute<ReadOnlyInput>() != null))
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

        if (dataRole == "calendarpicker")
        {
            output.Attributes.Add("data-format",
                CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern.Replace("y", "Y"));
        }

        if (IsCheckbox) dataRole = "checkbox";
        if (IsTextarea) dataRole = "textarea";
        if (IsSelect) dataRole = "select";
        output.Attributes.Add("data-role", dataRole);
        return Task.CompletedTask;
    }

    protected virtual async Task<string> GetLabelAsHtmlAsync(TagHelperOutput inputTag)
    {
        if (IsOutputHidden(inputTag) || IsCheckbox)
        {
            return string.Empty;
        }

        var resolvedLabelText = await GetLabelDisplayNameAsync();

        var label = new TagBuilder("label");

        label.InnerHtml.AppendHtml(
            await Localizer.GetLocalizedTextAsync(resolvedLabelText, AspFor.ModelExplorer));
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

        if (!FormContent.Horizontal) return label.ToHtmlString();
        var width = LabelWidth ?? FormContent.LabelWidth;
        await AddStyleAsync(label, $"min-width:{width}px;");

        return label.ToHtmlString();
    }

    protected virtual async Task<string> GetLabelDisplayNameAsync()
    {
        if (!string.IsNullOrWhiteSpace(Label)) return Label;

        var resolvedLabelText = Label ??
                                AspFor.Metadata.DisplayName ??
                                AspFor.Metadata.PropertyName;
        var expression = AspFor.Name;
        if (resolvedLabelText == null && expression != null)
        {
            var index = expression.LastIndexOf('.');
            // Expression does not contain a dot separator.
            resolvedLabelText = index == -1 ? expression : expression[(index + 1)..];
        }

        if (string.IsNullOrEmpty(Label))
        {
            Label = Name ?? AspFor.Name;
            //return await GetLabelAsHtmlUsingTagHelperAsync(context, output) ;
        }

        Label = await Localizer.GetLocalizedTextAsync(resolvedLabelText, AspFor.ModelExplorer);
        return Label;
    }

    protected virtual bool IsOutputHidden(TagHelperOutput inputTag)
    {
        return inputTag.Attributes.Any(a =>
            a.Name.ToLowerInvariant() == "type" && a.Value.ToString()?.ToLowerInvariant() == "hidden");
    }

    protected virtual string GetRequiredSymbol()
    {
        if (!DisplayRequiredSymbol)
        {
            return "";
        }

        if (!FormContent.RequiredSymbols)
        {
            return "";
        }

        return AspFor.ModelExplorer.GetAttribute<RequiredAttribute>() != null
            ? "<span class='fg-red'> * </span>"
            : "";
    }


    protected virtual Task SetInputValidatorAsync(TagHelperAttributeList attributes)
    {
        var validateAttributes = attributes.Where(m => m.Name?.StartsWith("data-val-") ?? false);
        var validateAttributeValue = "";
        foreach (var attribute in validateAttributes)
        {
            var name = attribute.Name?.Replace("data-val-", "");
            if (string.IsNullOrWhiteSpace(name) || name?.IndexOf("-") > 0) continue;
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

    protected virtual async Task AddPlaceholderAttributeAsync(TagHelperOutput inputTagHelperOutput)
    {
        if (inputTagHelperOutput.Attributes.ContainsName("placeholder"))
        {
            return;
        }

        var attribute = AspFor.ModelExplorer.GetAttribute<Placeholder>();

        if (attribute == null) return;
        var placeholderLocalized =
            await Localizer.GetLocalizedTextAsync(attribute.Value, AspFor.ModelExplorer);

        inputTagHelperOutput.Attributes.Add("placeholder", placeholderLocalized);
    }

    protected virtual async Task<bool> AddItemToFormItemsContents(TagHelperContext context, string propertyName,
        string html, int displayOrder)
    {
        if (!await HasGroupItemsAsync(context)) return false;
        var list = await GetGroupItemsAsync(context);
        if (list == null) return false;
        await AddGroupItemAsync(list, propertyName, html, displayOrder);
        return true;
    }
}