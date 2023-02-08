using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class SelectItems : Attribute
{
    public string ItemsListPropertyName { get; set; }

    public SelectItems(string itemsListPropertyName)
    {
        ItemsListPropertyName = itemsListPropertyName;
    }

    public IEnumerable<SelectListItem> GetItems(ModelExplorer explorer)
    {
        var properties = explorer.Container.Properties.Where(p =>
            p.Metadata.PropertyName != null && p.Metadata.PropertyName.Equals(ItemsListPropertyName)).ToList();

        while (properties.Count == 0)
        {
            explorer = explorer.Container;
            if (explorer.Container == null)
            {
                return null;
            }

            properties = explorer.Container.Properties.Where(p =>
                p.Metadata.PropertyName != null && p.Metadata.PropertyName.Equals(ItemsListPropertyName)).ToList();
        }

        var selectItems = (properties.First().Model as IEnumerable<SelectListItem> ?? Array.Empty<SelectListItem>())
            .ToList();

        return selectItems;
    }
}