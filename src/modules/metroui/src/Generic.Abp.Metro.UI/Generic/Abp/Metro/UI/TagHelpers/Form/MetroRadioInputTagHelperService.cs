using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroRadioInputTagHelperService : MetroTagHelperService<MetroRadioInputTagHelper>
{
    private readonly IMetroTagHelperLocalizer _tagHelperLocalizer;

    public MetroRadioInputTagHelperService(IMetroTagHelperLocalizer tagHelperLocalizer)
    {
        _tagHelperLocalizer = tagHelperLocalizer;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var selectItems = GetSelectItems(context, output);
        SetSelectedValue(context, output, selectItems);

        var order = TagHelper.AspFor.ModelExplorer.GetDisplayOrder();

        var html = GetHtml(context, output, selectItems);

        await AddItemToItemsAsync<FormItem>(context, FormItems, TagHelper.AspFor.Name);

        output.TagName = "div";
        output.Attributes.Clear();
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Content.SetHtmlContent(html);
    }

    protected virtual string GetHtml(TagHelperContext context, TagHelperOutput output, List<SelectListItem> selectItems)
    {
        var html = new StringBuilder("");

        foreach (var selectItem in selectItems)
        {
            var inlineClass = (TagHelper.Inline ?? false) ? " form-check-inline" : "";
            var id = TagHelper.AspFor.Name + "Radio" + selectItem.Value;
            var name = TagHelper.AspFor.Name;

            var input = new TagBuilder("input");
            input.AddCssClass("form-check-input");
            input.Attributes.Add("type", "radio");
            input.Attributes.Add("id", id);
            input.Attributes.Add("name", name);
            input.Attributes.Add("value", selectItem.Value);

            if (selectItem.Selected)
            {
                input.Attributes.Add("checked", "checked");
            }

            if (TagHelper.Disabled ?? false)
            {
                input.Attributes.Add("disabled", "disabled");
            }

            var label = new TagBuilder("label");
            label.AddCssClass("form-check-label");
            label.Attributes.Add("for", id);
            label.InnerHtml.AppendHtml(selectItem.Text);

            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("form-check" + inlineClass);
            wrapper.InnerHtml.AppendHtml(input);
            wrapper.InnerHtml.AppendHtml(label);

            html.AppendLine(wrapper.ToHtmlString());
        }

        return html.ToString();
    }

    protected virtual List<SelectListItem> GetSelectItems(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.AspItems != null)
        {
            return TagHelper.AspItems.ToList();
        }

        if (TagHelper.AspFor.ModelExplorer.Metadata.IsEnum)
        {
            return GetSelectItemsFromEnum(context, output, TagHelper.AspFor.ModelExplorer);
        }

        var selectItemsAttribute = TagHelper.AspFor.ModelExplorer.GetAttribute<SelectItems>();
        if (selectItemsAttribute != null)
        {
            return GetSelectItemsFromAttribute(selectItemsAttribute, TagHelper.AspFor.ModelExplorer);
        }

        throw new Exception("No items provided for select attribute.");
    }

    protected virtual List<SelectListItem> GetSelectItemsFromEnum(TagHelperContext context, TagHelperOutput output, ModelExplorer explorer)
    {
        var localizer = _tagHelperLocalizer.GetLocalizerOrNullAsync(explorer).GetAwaiter().GetResult();

        var selectItems = explorer.Metadata.IsEnum ? explorer.ModelType.GetTypeInfo().GetMembers(BindingFlags.Public | BindingFlags.Static)
            .Select((t, i) => new SelectListItem { Value = i.ToString(), Text = GetLocalizedPropertyName(localizer, explorer.ModelType, t.Name) }).ToList() : null;

        return selectItems;
    }

    protected virtual string GetLocalizedPropertyName(IStringLocalizer localizer, Type enumType, string propertyName)
    {
        if (localizer == null)
        {
            return propertyName;
        }

        var localizedString = localizer[enumType.Name + "." + propertyName];

        return !localizedString.ResourceNotFound ? localizedString.Value : localizer[propertyName].Value;
    }

    protected virtual List<SelectListItem> GetSelectItemsFromAttribute(
        SelectItems selectItemsAttribute,
        ModelExplorer explorer)
    {
        var selectItems = selectItemsAttribute.GetItems(explorer)?.ToList();

        if (selectItems == null)
        {
            return new List<SelectListItem>();
        }

        return selectItems;
    }

    protected virtual void SetSelectedValue(TagHelperContext context, TagHelperOutput output, List<SelectListItem> selectItems)
    {
        var selectedValue = GetSelectedValue(context, output);

        if (!selectItems.Any(si => si.Selected))
        {
            var itemToBeSelected = selectItems.FirstOrDefault(si => si.Value == selectedValue);

            if (itemToBeSelected != null)
            {
                itemToBeSelected.Selected = true;
            }
        }
    }

    protected virtual string GetSelectedValue(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.AspFor.ModelExplorer.Metadata.IsEnum)
        {
            var baseType = TagHelper.AspFor.ModelExplorer.Model?.GetType().GetEnumUnderlyingType();

            if (baseType == null)
            {
                return null;
            }

            var valueAsString = Convert.ChangeType(TagHelper.AspFor.ModelExplorer.Model, baseType);
            return valueAsString != null ? valueAsString.ToString() : "";
        }

        return TagHelper.AspFor.ModelExplorer.Model?.ToString();
    }

}
