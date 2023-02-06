using NUglify.JavaScript.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Razor.
    TagHelpers //TODO: Move to AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers namespace

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
            var existingClasses = classAttribute.Value.ToString()
                ?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
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

        var classList = classAttribute.Value.ToString()?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .ToList();
        classList?.RemoveAll(c => c == className);

        attributes.SetAttribute("class", classList.JoinAsString(" "));
    }

    public static void AddSizeStyle(this TagHelperAttributeList attributes, string value, bool isWidth = true)
    {
        var name = isWidth ? "width" : "height";
        if (string.IsNullOrWhiteSpace(value)) return;
        if (value.EndsWith("%"))
        {
            AddStyle(attributes, name, value);
        }

        if (value.EndsWith("px"))
        {
            AddStyle(attributes, name, value);
        }

        AddStyle(attributes, name, value + "px");
    }


    public static void AddStyle(this TagHelperAttributeList attributes, string name, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var classAttribute = attributes["style"];
        var style = $"{name}:{value};";
        if (classAttribute == null)
        {
            attributes.Add("style", style);
        }
        else
        {
            var existingStyle = classAttribute.Value.ToString()
                ?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (existingStyle == null)
            {
                attributes.Add("style", style);
                return;
            }

            if (existingStyle.Any(m => m.StartsWith(name)))
            {
                return;
            }

            existingStyle.Add(style);
            attributes.SetAttribute("style", string.Join(" ", existingStyle));
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