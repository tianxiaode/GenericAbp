using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class SelectItemsService: ITransientDependency
{
    public Task<bool> IsEnumAsync(ISelectItemsTagHelper tagHelper)
    {
        var value = tagHelper.AspFor.Model;
        if (value != null && value.GetType().IsEnum)
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(tagHelper.AspFor.ModelExplorer.Metadata.IsEnum);

    }

    public virtual async Task<List<SelectListItem>> GetSelectItemsAsync(ISelectItemsTagHelper tagHelper, IMetroTagHelperLocalizer tagHelperLocalizer, IAbpEnumLocalizer abpEnumLocalizer, IStringLocalizerFactory stringLocalizerFactory)
    {
        if (tagHelper.AspItems != null)
        {
            return tagHelper.AspItems.ToList();
        }

        if (await IsEnumAsync(tagHelper))
        {
            return await GetSelectItemsFromEnumAsync(tagHelper.AspFor.ModelExplorer, tagHelperLocalizer, abpEnumLocalizer, stringLocalizerFactory);
        }

        var selectItemsAttribute = tagHelper.AspFor.ModelExplorer.GetAttribute<SelectItems>();
        if (selectItemsAttribute != null)
        {
            return await GetSelectItemsFromAttribute(selectItemsAttribute, tagHelper.AspFor.ModelExplorer);
        }

        throw new Exception("No items provided for select attribute.");
    }

    public virtual async Task<List<SelectListItem>> GetSelectItemsFromEnumAsync(ModelExplorer explorer, IMetroTagHelperLocalizer tagHelperLocalizer, IAbpEnumLocalizer abpEnumLocalizer, IStringLocalizerFactory stringLocalizerFactory)
    {
        var selectItems = new List<SelectListItem>();
        var isNullableType = Nullable.GetUnderlyingType(explorer.ModelType) != null;
        var enumType = explorer.ModelType;

        if (isNullableType)
        {
            enumType = Nullable.GetUnderlyingType(explorer.ModelType);
            selectItems.Add(new SelectListItem());
        }

        var containerLocalizer = await tagHelperLocalizer.GetLocalizerOrNullAsync(explorer.Container.ModelType.Assembly);
        if (enumType == null) return selectItems;
        selectItems.AddRange(from object enumValue in enumType.GetEnumValuesAsUnderlyingType()
            let localizedMemberName =
                abpEnumLocalizer.GetString(enumType, enumValue,
                    new[] { containerLocalizer, stringLocalizerFactory.CreateDefaultOrNull() })
            select new SelectListItem { Value = enumValue.ToString(), Text = localizedMemberName });

        return selectItems;
    }

    public virtual Task<List<SelectListItem>> GetSelectItemsFromAttribute(
        SelectItems selectItemsAttribute,
        ModelExplorer explorer)
    {
        var selectItems = selectItemsAttribute.GetItems(explorer)?.ToList();

        return Task.FromResult(selectItems ?? new List<SelectListItem>());
    }

    public virtual async Task SetSelectedValueAsync(List<SelectListItem> selectItems,ISelectItemsTagHelper tagHelper)
    {
        var selectedValue = await GetSelectedValueAsync(tagHelper);

        if (!selectItems.Any(si => si.Selected))
        {
            var itemToBeSelected = selectItems.FirstOrDefault(si => si.Value == selectedValue);

            if (itemToBeSelected != null)
            {
                itemToBeSelected.Selected = true;
            }
        }
    }

    protected virtual Task<string> GetSelectedValueAsync(ISelectItemsTagHelper tagHelper)
    {
        if (!tagHelper.AspFor.ModelExplorer.Metadata.IsEnum) return Task.FromResult(tagHelper?.AspFor?.ModelExplorer?.Model?.ToString());
        var baseType = tagHelper?.AspFor?.ModelExplorer?.Model?.GetType()?.GetEnumUnderlyingType();

        if (baseType == null)
        {
            return null;
        }

        var valueAsString = Convert.ChangeType(tagHelper?.AspFor?.ModelExplorer?.Model, baseType);
        return Task.FromResult(valueAsString != null ? valueAsString.ToString() : "");

    }


}