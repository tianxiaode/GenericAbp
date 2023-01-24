using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Nito.AsyncEx;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroRadioInputTagHelperService : MetroInputTagHelperServiceBase<MetroRadioInputTagHelper>
{
    protected IMetroTagHelperLocalizer TagHelperLocalizer { get; }
    protected SelectItemsService SelectItemsService { get; }
    protected IStringLocalizerFactory StringLocalizerFactory { get; }
    protected IAbpEnumLocalizer AbpEnumLocalizer { get; }
    protected string ColumnClass { get; set; }

    public MetroRadioInputTagHelperService(IHtmlGenerator generator, HtmlEncoder encoder, IMetroTagHelperLocalizer tagHelperLocalizer, SelectItemsService selectItemsService, IStringLocalizerFactory stringLocalizerFactory, IAbpEnumLocalizer abpEnumLocalizer) : base(generator, encoder)
    {
        TagHelperLocalizer = tagHelperLocalizer;
        SelectItemsService = selectItemsService;
        StringLocalizerFactory = stringLocalizerFactory;
        AbpEnumLocalizer = abpEnumLocalizer;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await SetRadioColumnsClassAsync();
        await GetFormContentAsync(context);
        IsRadio = true;
        var selectItems = await SelectItemsService.GetSelectItemsAsync(TagHelper,TagHelperLocalizer, AbpEnumLocalizer, StringLocalizerFactory);
        await SelectItemsService.SetSelectedValueAsync(selectItems, TagHelper);

        var order = TagHelper.AspFor.ModelExplorer.GetDisplayOrder();

        var html = await GetHtmlAsync(context, output, selectItems);
        var label = await GetLabelAsHtmlAsync(output, TagHelperLocalizer);

        await AddItemToItemsAsync<FormItem>(context, FormItems, TagHelper.AspFor.Name);

        output.TagName = "div";
        output.Attributes.Clear();
        output.TagMode = TagMode.StartTagAndEndTag;
        await SetInputSizeAsync(output);
        output.Content.SetHtmlContent(await GetContentAsync(label, html));
        if (!string.IsNullOrWhiteSpace(TagHelper.AspFor?.Name))
        {
            await AddItemToItemsAsync<FormItem>(context, FormItems, TagHelper.AspFor.Name);
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
            var attribute = TagHelper.AspFor.ModelExplorer.GetAttribute<MetroRadioColumns>();
            if (attribute != null) cols = attribute.Columns;
        }

        if (cols == 0) cols = 1;
        cols = 12 / cols;
        ColumnClass = $"cell-{cols}";
        return Task.CompletedTask;
    }

}
