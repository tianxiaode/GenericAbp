using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroSelectTagHelperService : MetroInputTagHelperServiceBase<MetroSelectTagHelper>
{
    protected IMetroTagHelperLocalizer TagHelperLocalizer { get; }
    protected SelectItemsService SelectItemsService { get; }
    protected IStringLocalizerFactory StringLocalizerFactory { get; }
    protected IAbpEnumLocalizer AbpEnumLocalizer { get; }

    public MetroSelectTagHelperService(IHtmlGenerator generator, HtmlEncoder encoder, IMetroTagHelperLocalizer tagHelperLocalizer, SelectItemsService selectItemsService, IStringLocalizerFactory stringLocalizerFactory, IAbpEnumLocalizer abpEnumLocalizer) : base(generator, encoder)
    {
        TagHelperLocalizer = tagHelperLocalizer;
        SelectItemsService = selectItemsService;
        StringLocalizerFactory = stringLocalizerFactory;
        AbpEnumLocalizer = abpEnumLocalizer;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        IsSelect = true;
        await GetFormContentAsync(context);
        await SetOrderAsync(output);
        var childContent = await output.GetChildContentAsync();
        var innerHtml = await GetFormInputGroupAsHtmlAsync(context, output, childContent);


        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Content.SetHtmlContent(innerHtml);
        await SetInputSizeAsync(output);

        var suppress =await AddItemToFromItemsAsync(context, FormItems, TagHelper.AspFor.Name, Order, await output.RenderAsync(Encoder));
        
        if (suppress)
        {
            
            output.SuppressOutput();
        }
    }

    protected virtual async Task<string> GetFormInputGroupAsHtmlAsync(TagHelperContext context, TagHelperOutput output,TagHelperContent childContent )
    {
        var selectTag = await GetSelectTagAsync(context, output, childContent);
        var selectAsHtml = await selectTag.RenderAsync(Encoder);
        var label = await GetLabelAsHtmlAsync(selectTag,TagHelperLocalizer);

        return label + Environment.NewLine + selectAsHtml;
    }

    protected virtual async Task<TagHelperOutput> GetSelectTagAsync(TagHelperContext context, TagHelperOutput output, TagHelperContent childContent)
    {
        var selectTagHelper = new SelectTagHelper(Generator)
        {
            For = TagHelper.AspFor,
            ViewContext = TagHelper.ViewContext
        };

        if (TagHelper.AutocompleteApiUrl.IsNullOrEmpty())
        {
            selectTagHelper.Items = await SelectItemsService.GetSelectItemsAsync(TagHelper, TagHelperLocalizer, AbpEnumLocalizer, StringLocalizerFactory);
        }
        else if (!TagHelper.AutocompleteSelectedItemName.IsNullOrEmpty())
        {
            selectTagHelper.Items = new[]
            {
                new SelectListItem(TagHelper.AutocompleteSelectedItemName,
                    TagHelper.AutocompleteSelectedItemValue, false)
            };
        }

        var selectTagHelperOutput = await selectTagHelper.ProcessAndGetOutputAsync(new TagHelperAttributeList(), context, "select", TagMode.StartTagAndEndTag);

        selectTagHelperOutput.Content.SetHtmlContent(childContent);
        selectTagHelperOutput.Attributes.AddClass("metro-input");
        await AddPlaceholderAttributeAsync(selectTagHelperOutput, TagHelperLocalizer);
        AddDisabledAttribute(selectTagHelperOutput);
        AddReadOnlyAttribute(selectTagHelperOutput);
        await SetDataRoleAttributeAsync(selectTagHelperOutput);
        await SetInputValidatorAsync(selectTagHelperOutput.Attributes);
        

        return selectTagHelperOutput;
    }


}
