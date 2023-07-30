using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroSelectTagHelper : MetroInputTagHelperBase, ISelectItemsTagHelper
{
    public MetroSelectTagHelper(HtmlEncoder htmlEncoder,
        IHtmlGenerator generator,
        IMetroTagHelperLocalizerService localizer,
        SelectItemsService selectItemsService) :
        base(htmlEncoder, generator, localizer)
    {
        SelectItemsService = selectItemsService;
        IsSelect = true;
    }

    protected SelectItemsService SelectItemsService { get; }
    public IEnumerable<SelectListItem> AspItems { get; set; }
    [HtmlAttributeName("info")] public string InfoText { get; set; }
    public string AutocompleteApiUrl { get; set; }
    public string AutocompleteItemsPropertyName { get; set; }
    public string AutocompleteDisplayPropertyName { get; set; }
    public string AutocompleteValuePropertyName { get; set; }
    public string AutocompleteFilterParamName { get; set; }
    public string AutocompleteSelectedItemName { get; set; }
    public string AutocompleteSelectedItemValue { get; set; }
    public string AllowClear { get; set; }
    public string Placeholder { get; set; }
    public bool Filter { get; set; } = true;

    protected override async Task<string> GetFormInputGroupAsHtmlAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = await output.GetChildContentAsync();
        var selectTag = await GetSelectTagAsync(context, output, childContent);
        var selectAsHtml = await selectTag.RenderAsync(HtmlEncoder);
        var label = await GetLabelAsHtmlAsync(selectTag);

        return label + Environment.NewLine + selectAsHtml;
    }

    protected virtual async Task<TagHelperOutput> GetSelectTagAsync(TagHelperContext context, TagHelperOutput output,
        TagHelperContent childContent)
    {
        var selectTagHelper = new SelectTagHelper(Generator)
        {
            For = AspFor,
            ViewContext = ViewContext,
            Items = await SelectItemsService.GetSelectItemsAsync(this, Localizer)
        };

        var selectTagHelperOutput = await selectTagHelper.ProcessAndGetOutputAsync(new TagHelperAttributeList(),
            context, "select", TagMode.StartTagAndEndTag);

        selectTagHelperOutput.Content.SetHtmlContent(childContent);
        selectTagHelperOutput.Attributes.AddClass("metro-input");
        await SetInputSizeAsync(selectTagHelperOutput);
        await AddPlaceholderAttributeAsync(selectTagHelperOutput);
        AddDisabledAttribute(selectTagHelperOutput);
        AddReadOnlyAttribute(selectTagHelperOutput);
        await SetDataRoleAttributeAsync(selectTagHelperOutput);
        await SetInputValidatorAsync(selectTagHelperOutput.Attributes);
        await AddPrependAndAppendAttributesAsync(selectTagHelperOutput);

        if (!Filter)
        {
            selectTagHelperOutput.Attributes.Add("data-filter", false);
        }

        return selectTagHelperOutput;
    }

    protected virtual void AddAutocompleteAttributes(TagHelperOutput output)
    {
        if (AutocompleteApiUrl.IsNullOrEmpty()) return;
        var attributes = output.Attributes;
        attributes.Add("data-autocomplete-api-url", AutocompleteApiUrl);
        attributes.Add("data-autocomplete-items-property", AutocompleteItemsPropertyName);
        attributes.Add("data-autocomplete-display-property", AutocompleteDisplayPropertyName);
        attributes.Add("data-autocomplete-value-property", AutocompleteValuePropertyName);
        attributes.Add("data-autocomplete-filter-param-name", AutocompleteFilterParamName);
        attributes.Add("data-autocomplete-selected-item-name", AutocompleteSelectedItemName);
        attributes.Add("data-autocomplete-selected-item-value", AutocompleteSelectedItemValue);
        attributes.Add("data-autocomplete-allow-clear", AllowClear);
        attributes.Add("data-autocomplete-placeholder", Placeholder);
    }
}