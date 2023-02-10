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
            ViewContext = ViewContext
        };

        selectTagHelper.Items =
            await SelectItemsService.GetSelectItemsAsync(this, Localizer);

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


        return selectTagHelperOutput;
    }
}