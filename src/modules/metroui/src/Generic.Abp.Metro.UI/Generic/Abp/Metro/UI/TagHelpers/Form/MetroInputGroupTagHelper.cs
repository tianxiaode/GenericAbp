using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroInputGroupTagHelper : MetroInputTagHelperBase, ISelectItemsTagHelper
{
    public MetroInputGroupTagHelper(
        HtmlEncoder htmlEncoder,
        IHtmlGenerator generator,
        IMetroTagHelperLocalizerService localizer,
        SelectItemsService selectItemsService) : base(htmlEncoder, generator, localizer)
    {
        SelectItemsService = selectItemsService;
    }

    protected SelectItemsService SelectItemsService { get; }
    public IEnumerable<SelectListItem> AspItems { get; set; }

    public int? Cols { get; set; }

    protected override async Task<string> GetFormInputGroupAsHtmlAsync(TagHelperContext context, TagHelperOutput output)
    {
        var builder = new StringBuilder();
        builder.AppendLine(await GetLabelAsHtmlAsync(output));
        builder.AppendLine("$<div class=\"row w-100\">");
        if (IsCheckboxGroup || IsRadioGroup)
        {
            builder.AppendLine(await GetInputGroupAsHtmlAsync(context, output));
        }
        else
        {
            var child = await output.GetChildContentAsync();
            builder.AppendLine(child.GetContent());
        }

        builder.AppendLine("</div>");
        return builder.ToString();
    }

    protected virtual async Task<string> GetInputGroupAsHtmlAsync(TagHelperContext context, TagHelperOutput output)
    {
        var html = new StringBuilder("");
        var type = IsRadioGroup ? "radio" : "checkbox";
        var colsCls = await GetGroupColsClsAsync();
        var list = AspItems?.ToList() ?? await SelectItemsService.GetSelectItemsAsync(this, Localizer);
        if (list == null) return "";
        await SetSelectedValueAsync(list);

        foreach (var selectItem in list)
        {
            var id = AspFor.Name + type + selectItem.Value;
            var name = AspFor.Name;

            var input = new TagBuilder("input");
            var attributes = input.Attributes;
            attributes.Add("type", type);
            attributes.Add("id", id);
            attributes.Add("name", name);
            attributes.Add("value", selectItem.Value);
            attributes.Add("data-role", "checkbox");
            attributes.Add("data-style", "2");
            attributes.Add("class", colsCls);
            //attributes.Add(" data-cls-caption", "fg-cyan text-bold");
            //attributes.Add(" data-cls-check", "bd-cyan");

            if (selectItem.Selected)
            {
                attributes.Add("checked", "checked");
            }

            AddDisabledAttribute(output);

            attributes.Add("data-caption", selectItem.Text);

            html.AppendLine(input.ToHtmlString());
        }

        return html.ToString();
    }

    protected virtual Task<string> GetGroupColsClsAsync()
    {
        var cols = 0;
        if (Cols.HasValue) cols = Cols.Value;
        if (cols == 0)
        {
            var attribute = AspFor.ModelExplorer.GetAttribute<RadioOrCheckboxCols>();
            if (attribute != null) cols = attribute.Cols;
        }

        if (cols == 0) cols = 1;
        cols = 12 / cols;
        return Task.FromResult($"cell-{cols}");
    }

    public virtual Task SetSelectedValueAsync(List<SelectListItem> selectItems)
    {
        var explorer = AspFor.ModelExplorer;
        var metadata = explorer.Metadata;
        var type = metadata.ElementType;
        var model = explorer.Model;
        if (model == null) return Task.CompletedTask;
        foreach (var o in (IEnumerable)model)
        {
            var isEnum = type?.IsEnum ?? false;
            var strValue = o.ToString();
            if (isEnum)
            {
                var baseType = o.GetType()?.GetEnumUnderlyingType();
                var valueAsString = Convert.ChangeType(o, baseType);
                strValue = valueAsString.ToString() ?? "";
            }

            if (string.IsNullOrWhiteSpace(strValue)) continue;
            var item = selectItems.FirstOrDefault(m => m.Value.Equals(strValue));
            if (item == null) continue;
            item.Selected = true;
        }

        return Task.CompletedTask;
    }
}