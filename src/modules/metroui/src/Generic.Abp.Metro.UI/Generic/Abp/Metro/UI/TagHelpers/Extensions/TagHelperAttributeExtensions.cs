using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Extensions;

public static class TagHelperAttributeExtensions
{
    public static Task<string> ToHtmlAttributeAsStringAsync(this TagHelperAttribute attribute)
    {
        return Task.FromResult(attribute.Name + "=\"" + attribute.Value + "\"");
    }

    public static Task<string> ToHtmlAttributesAsStringAsync(this List<TagHelperAttribute> attributes)
    {
        var attributesAsString = attributes.Aggregate("", (current, attribute) => current + (attribute.ToHtmlAttributeAsStringAsync() + " "));

        return Task.FromResult(attributesAsString);
    }
}
