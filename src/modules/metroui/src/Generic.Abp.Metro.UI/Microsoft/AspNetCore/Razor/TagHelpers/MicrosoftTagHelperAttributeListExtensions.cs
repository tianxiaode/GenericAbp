using NUglify.JavaScript.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Razor.TagHelpers //TODO: Move to AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers namespace
;

public static class MicrosoftTagHelperAttributeListExtensions
{
    public static void AddClass(this TagHelperAttributeList attributes, string className)
    {
        if (string.IsNullOrWhiteSpace(className))
        {
            return;
        }

        var classAttribute = attributes["class"];
        if (classAttribute == null)
        {
            attributes.Add("class", className);
        }
        else
        {
            var existingClasses = classAttribute.Value.ToString()?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            existingClasses.AddIfNotContains(className);
            if (existingClasses != null) attributes.SetAttribute("class", string.Join(" ", existingClasses));
        }
    }

    public static void RemoveClass(this TagHelperAttributeList attributes, string className)
    {
        if (string.IsNullOrWhiteSpace(className))
        {
            return;
        }

        var classAttribute = attributes["class"];
        if (classAttribute == null)
        {
            return;
        }

        var classList = classAttribute.Value.ToString()?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        classList?.RemoveAll(c => c == className);

        attributes.SetAttribute("class", classList.JoinAsString(" "));
    }

    public static void AddStyle(this TagHelperAttributeList attributes, string styleName, string styleValue)
    {
        if (string.IsNullOrWhiteSpace(styleName) || string.IsNullOrWhiteSpace(styleValue))
        {
            return;
        }

        var newStyle =$"{styleName}:{styleValue};";
        attributes.Add("style", newStyle);
        return;

        var styleAttribute = attributes["style"];
        if (styleAttribute == null)
        {
            attributes.Add("style", newStyle);
        }
        else
        {
            var oldStyle = styleAttribute.Value.ToString().EnsureEndsWith(';');
            var existingStyle = oldStyle.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if(existingStyle.Any(m=>m.StartsWith(styleName))) return;
            var style = oldStyle + newStyle;
            attributes.Add("style", style);

        }

    }


    public static void AddIfNotContains(this TagHelperAttributeList attributes, string name, object value)
    {
        if (!attributes.ContainsName(name))
        {
            attributes.Add(name, value);
        }
    }
}
