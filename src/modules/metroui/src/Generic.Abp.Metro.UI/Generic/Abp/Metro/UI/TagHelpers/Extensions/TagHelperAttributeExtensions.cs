using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Extensions;

public static class TagHelperAttributeExtensions
{
    public static string ToHtmlAttributeAsString(this TagHelperAttribute attribute)
    {
        return attribute.Name + "=\"" + attribute.Value + "\"";
    }

    public static string ToHtmlAttributesAsString(this List<TagHelperAttribute> attributes)
    {
        var attributesAsString = "";

        foreach (var attribute in attributes)
        {
            attributesAsString += attribute.ToHtmlAttributeAsString() + " ";
        }

        return attributesAsString;
    }
}
