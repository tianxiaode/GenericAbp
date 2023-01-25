using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroRadioGroupTagHelperService : MetroInputTagHelperServiceBase<MetroRadioGroupTagHelper>
{
    protected IMetroTagHelperLocalizer TagHelperLocalizer { get; }
    protected SelectItemsService SelectItemsService { get; }
    protected IStringLocalizerFactory StringLocalizerFactory { get; }
    protected IAbpEnumLocalizer AbpEnumLocalizer { get; }
    protected string ColumnClass { get; set; }

    public MetroRadioGroupTagHelperService(IHtmlGenerator generator, HtmlEncoder encoder, IMetroTagHelperLocalizer tagHelperLocalizer, SelectItemsService selectItemsService, IStringLocalizerFactory stringLocalizerFactory, IAbpEnumLocalizer abpEnumLocalizer) : base(generator, encoder)
    {
        TagHelperLocalizer = tagHelperLocalizer;
        SelectItemsService = selectItemsService;
        StringLocalizerFactory = stringLocalizerFactory;
        AbpEnumLocalizer = abpEnumLocalizer;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await SetRadioColumnsClassAsync();
        await SetOrderAsync(output);
        await GetFormContentAsync(context);
        IsRadio = true;
        var selectItems = await SelectItemsService.GetSelectItemsAsync(TagHelper,TagHelperLocalizer, AbpEnumLocalizer, StringLocalizerFactory);
        await SetSelectedValueAsync(selectItems);

        var html = await GetHtmlAsync(context, output, selectItems);
        var label = await GetLabelAsHtmlAsync(output, TagHelperLocalizer);


        output.TagName = "div";
        output.Attributes.Clear();
        output.TagMode = TagMode.StartTagAndEndTag;
        await SetInputSizeAsync(output);
        output.Content.SetHtmlContent(await GetContentAsync(label, html));
        var suppress =await AddItemToFromItemsAsync(context, FormItems, TagHelper.AspFor.Name, Order, await output.RenderAsync(Encoder));
        
        if (suppress)
        {
            
            output.SuppressOutput();
        }
    }

    protected virtual Task<string> GetContentAsync(string label, string inputHtml)
    {
        var inputDiv = $"<div class=\"row w-100\">{inputHtml}</div>";
        return Task.FromResult(label + inputDiv);
    }

    protected virtual Task<string> GetHtmlAsync(TagHelperContext context, TagHelperOutput output, List<SelectListItem> selectItems)
    {
        var html = new StringBuilder("");

        foreach (var selectItem in selectItems)
        {
            var id = TagHelper.AspFor.Name + "Radio" + selectItem.Value;
            var name = TagHelper.AspFor.Name;

            var input = new TagBuilder("input");
            var attributes = input.Attributes;
            attributes.Add("type", "radio");
            attributes.Add("id", id);
            attributes.Add("name", name);
            attributes.Add("value", selectItem.Value);
            attributes.Add("data-role", "radio");
            attributes.Add("data-style", "2");
            attributes.Add("class", ColumnClass);
            //attributes.Add(" data-cls-caption", "fg-cyan text-bold");
            //attributes.Add(" data-cls-check", "bd-cyan");

            if (selectItem.Selected)
            {
                attributes.Add("checked", "checked");
            }

            AddDisabledAttribute(output);

            attributes.Add("data-caption",selectItem.Text);

            html.Append(input.ToHtmlString());
        }

        return Task.FromResult(html.ToString());
    }

    protected virtual Task SetRadioColumnsClassAsync()
    {
        var cols = 0;
        if (TagHelper.Cols.HasValue) cols = TagHelper.Cols.Value;
        if (cols == 0)
        {
            var attribute = TagHelper.AspFor.ModelExplorer.GetAttribute<MetroRadioOrCheckboxCols>();
            if (attribute != null) cols = attribute.Cols;
        }

        if (cols == 0)
        {
            var attribute = TagHelper.AspFor.ModelExplorer.GetAttribute<MetroCheckboxGroup>();
            if (attribute != null) cols = attribute.Cols;

        }

        if (cols == 0)
        {
            var attribute = TagHelper.AspFor.ModelExplorer.GetAttribute<MetroRadioGroup>();
            if (attribute != null) cols = attribute.Cols;

        }

        if (cols == 0) cols = 1;
        cols = 12 / cols;
        ColumnClass = $"cell-{cols}";
        return Task.CompletedTask;
    }

    public virtual async Task SetSelectedValueAsync(List<SelectListItem> selectItems)
    {
        var selectedValue = await GetSelectedValueAsync();

        if (!selectItems.Any(si => si.Selected))
        {
            var itemToBeSelected = selectItems.FirstOrDefault(si => si.Value == selectedValue);

            if (itemToBeSelected != null)
            {
                itemToBeSelected.Selected = true;
            }
        }
    }

    protected virtual Task<string> GetSelectedValueAsync()
    {
        if (!TagHelper.AspFor.ModelExplorer.Metadata.IsEnum) return Task.FromResult(TagHelper.AspFor.ModelExplorer.Model?.ToString());
        var baseType = TagHelper.AspFor.ModelExplorer.Model?.GetType()?.GetEnumUnderlyingType();

        if (baseType == null)
        {
            return null;
        }

        var valueAsString = Convert.ChangeType(TagHelper.AspFor.ModelExplorer.Model, baseType);
        return Task.FromResult(valueAsString != null ? valueAsString.ToString() : "");

    }

}
