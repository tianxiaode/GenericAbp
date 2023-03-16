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
    protected bool IsTagInput { get; set; } = false;
    protected bool IsHidden { get; set; } = false;
    public ModelExpression AspFor { get; set; }
    public bool Disabled { get; set; } = false;
    public bool Readonly { get; set; } = false;
    public string Label { get; set; }
    [HtmlAttributeName("type")] public string InputTypeName { get; set; }
    public bool? RequiredSymbol { get; set; }
    [HtmlAttributeName("asp-format")] public string Format { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public bool NoLabel { get; set; } = false;
    public int? LabelWidth { get; set; }
    public CheckBoxHiddenInputRenderMode? CheckBoxHiddenInputRenderMode { get; set; }
    public InputSize? Size { get; set; }
    public string Prepend { get; set; }
    public string Append { get; set; }
    public string ClsPrepend { get; set; }
    public string ClsAppend { get; set; }
    public int DisplayOrder { get; set; } = 0;

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
        var order = await GetDisplayOrderAsync(DisplayOrder);
        if (!IsHidden)
        {
            await SetColumnWidthAsync(output);
            await AddStyleAsync(output, $"order:{order};");
        }

        output.Content.AppendHtml(innerHtml);
        if (!string.IsNullOrWhiteSpace(GetIdAttributeValue(output))) output.Attributes.Remove(output.Attributes["id"]);
        var suppress = await AddItemToFormItemsContents(context, AspFor.Name,
            await output.RenderAsync(HtmlEncoder), order);
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
        FormContent = new FormContent();
        if (context.Items.ContainsKey(TagHelperConsts.FormContent))
        {
            var formContent = context.Items[TagHelperConsts.FormContent];
            if (formContent != null)
            {
                FormContent = formContent as FormContent ?? FormContent;
            }
        }

        if (LabelWidth.HasValue)
        {
            FormContent.LabelWidth = LabelWidth.Value;
        }

        if (RequiredSymbol.HasValue)
        {
            FormContent.RequiredSymbols = RequiredSymbol.Value;
        }

        if (Size.HasValue)
        {
            FormContent.Size = Size.Value;
        }
        else if (AspFor.ModelExplorer.GetAttribute<Attributes.InputSize>() != null)
        {
            FormContent.Size = AspFor.ModelExplorer.GetAttribute<Attributes.InputSize>().Size;
        }

        return Task.CompletedTask;
    }

    protected Task SetColumnWidthAsync(TagHelperOutput output)
    {
        var attributes = output.Attributes;
        var cols = FormContent.Cols;
        if (IsTextarea || IsCheckbox || IsRadio || IsCheckboxGroup || IsRadioGroup) cols = 1;
        if (FormContent is not { Horizontal: true }) return Task.CompletedTask;
        attributes.AddClass($"w-cols-{cols} pt-1 pb-6");
        attributes.AddClass("d-flex");
        return Task.CompletedTask;
    }

    protected async Task SetLabelSizeAsync(TagBuilder tagBuilder)
    {
        var size = await GetSizeStringAsync();
        //line-height
        tagBuilder.AddCssClass($"lh-input-label{size}");
    }

    protected async Task SetInputSizeAsync(TagHelperOutput output)
    {
        var size = await GetSizeStringAsync();
        if (string.IsNullOrWhiteSpace(size)) return;
        output.Attributes.AddClass($"input{size}");
    }

    protected Task<string> GetSizeStringAsync()
    {
        var size = FormContent.Size switch
        {
            InputSize.Large => "large",
            InputSize.Small => "small",
            _ => "",
        };
        if (!string.IsNullOrWhiteSpace(size)) size = "-" + size;
        return Task.FromResult(size);
    }


    protected virtual void AddDisabledAttribute(TagHelperOutput inputTagHelperOutput)
    {
        if (inputTagHelperOutput.Attributes.ContainsName("disabled") == false &&
            (Disabled || AspFor.ModelExplorer.GetAttribute<DisabledInput>() != null))
        {
            inputTagHelperOutput.Attributes.Add("disabled", "");
        }
    }

    protected virtual void AddReadOnlyAttribute(TagHelperOutput inputTagHelperOutput)
    {
        if (inputTagHelperOutput.Attributes.ContainsName("readonly") == false &&
            (Readonly || AspFor.ModelExplorer.GetAttribute<ReadOnlyInput>() != null))
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
            "hidden" => "",
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

        if (IsTagInput) dataRole = "taginput";
        if (IsCheckbox) dataRole = "checkbox";
        if (IsTextarea) dataRole = "textarea";
        if (IsSelect) dataRole = "select";
        output.Attributes.Add("data-role", dataRole);
        return Task.CompletedTask;
    }

    protected virtual async Task<string> GetLabelAsHtmlAsync(TagHelperOutput inputTag)
    {
        if (IsOutputHidden(inputTag) || IsCheckbox || NoLabel)
        {
            return string.Empty;
        }

        var resolvedLabelText = await GetLabelDisplayNameAsync();

        var label = new TagBuilder("label");
        label.AddCssClass("label-for-input");
        label.Attributes.Add("for", GetIdAttributeValue(inputTag));

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

    protected virtual async Task AddPrependAndAppendAttributesAsync(TagHelperOutput output)
    {
        if (AspFor.ModelExplorer.GetAttribute<InputPrepend>() != null)
        {
            Prepend = AspFor.ModelExplorer.GetAttribute<InputPrepend>().Prepend;
        }

        if (AspFor.ModelExplorer.GetAttribute<InputClsPrepend>() != null)
        {
            ClsPrepend = AspFor.ModelExplorer.GetAttribute<InputClsPrepend>().ClsPrepend;
        }

        if (AspFor.ModelExplorer.GetAttribute<InputAppend>() != null)
        {
            Append = AspFor.ModelExplorer.GetAttribute<InputAppend>().Append;
        }

        if (AspFor.ModelExplorer.GetAttribute<InputClsAppend>() != null)
        {
            ClsAppend = AspFor.ModelExplorer.GetAttribute<InputClsAppend>().ClsAppend;
        }


        if (!string.IsNullOrWhiteSpace(Prepend)) await AddDataAttributeAsync(output, nameof(Prepend), Prepend);
        if (!string.IsNullOrWhiteSpace(Append)) await AddDataAttributeAsync(output, nameof(Append), Append);
        if (!string.IsNullOrWhiteSpace(ClsPrepend))
            await AddDataAttributeAsync(output, nameof(ClsPrepend), ClsPrepend);
        if (!string.IsNullOrWhiteSpace(ClsAppend)) await AddDataAttributeAsync(output, nameof(ClsAppend), ClsAppend);
    }
}