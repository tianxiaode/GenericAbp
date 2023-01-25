using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class SelectItemsService: ITransientDependency
{
    public Task<bool> IsEnumAsync(ISelectItemsTagHelper tagHelper)
    {
        var modelExplorer = tagHelper.AspFor.ModelExplorer;
        var metadata = modelExplorer.Metadata;
        return !metadata.IsEnumerableType ? Task.FromResult(metadata.IsEnum) : Task.FromResult(metadata.ElementType?.IsEnum ?? false);
    }
    public virtual async Task<List<SelectListItem>> GetSelectItemsAsync(ISelectItemsTagHelper tagHelper, IMetroTagHelperLocalizer tagHelperLocalizer, IAbpEnumLocalizer abpEnumLocalizer, IStringLocalizerFactory stringLocalizerFactory)
    {
        if (tagHelper.AspItems != null)
        {
            return tagHelper.AspItems.ToList();
        }

        if (await IsEnumAsync(tagHelper))
        {
            return await GetSelectItemsFromEnumAsync(tagHelper, tagHelperLocalizer, abpEnumLocalizer, stringLocalizerFactory);
        }

        var selectItemsAttribute = tagHelper.AspFor.ModelExplorer.GetAttribute<SelectItems>();
        if (selectItemsAttribute != null)
        {
            return await GetSelectItemsFromAttribute(selectItemsAttribute, tagHelper.AspFor.ModelExplorer);
        }

        throw new Exception("No items provided for select attribute.");
    }

    public virtual async Task<List<SelectListItem>> GetSelectItemsFromEnumAsync(ISelectItemsTagHelper tagHelper,IMetroTagHelperLocalizer tagHelperLocalizer, IAbpEnumLocalizer abpEnumLocalizer, IStringLocalizerFactory stringLocalizerFactory)
    {
        var selectItems = new List<SelectListItem>();
        var modelExplorer = tagHelper.AspFor.ModelExplorer;
        var metadata = modelExplorer.Metadata;
        var enumType = modelExplorer.ModelType;
        if (metadata.IsEnumerableType)
        {
            enumType = metadata.ElementType;
        }
        var isNullableType = Nullable.GetUnderlyingType(modelExplorer.ModelType) != null;
        if (isNullableType)
        {
            enumType = Nullable.GetUnderlyingType(modelExplorer.ModelType);
            selectItems.Add(new SelectListItem());
        }
        var containerLocalizer = await tagHelperLocalizer.GetLocalizerOrNullAsync(modelExplorer.Container.ModelType.Assembly);
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



}