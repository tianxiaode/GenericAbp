using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Reflection;

namespace Generic.Abp.Metro.UI.TagHelpers.Extensions;

public static class ModelExplorerExtensions
{
    public static T GetAttribute<T>(this ModelExplorer property) where T : Attribute
    {
        if (property.Metadata.PropertyName != null)
            return property?.Metadata?.ContainerType?.GetTypeInfo()?.GetProperty(property.Metadata.PropertyName)
                ?.GetCustomAttribute<T>();
        return null;
    }

    //public static int GetDisplayOrder(this ModelExplorer explorer)
    //{
    //    return GetAttribute<DisplayOrder>(explorer)?.Number ?? DisplayOrder.Default;
    //}
}