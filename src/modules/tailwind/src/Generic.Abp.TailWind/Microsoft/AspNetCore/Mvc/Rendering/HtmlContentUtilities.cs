using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Tailwind.Microsoft.AspNetCore.Mvc.Rendering;

public class HtmlContentUtilities
{
    public static string HtmlContentToString(IHtmlContent content)
    {
        using var writer = new StringWriter();
        content.WriteTo(writer, NullHtmlEncoder.Default);
        return writer.ToString();
    }

    public static void AddAttributes(TagBuilder builder, object? attributes)
    {
        foreach (var controlAttribute in HtmlHelper.AnonymousObjectToHtmlAttributes(attributes))
        {
            builder.Attributes.Add(controlAttribute.Key, controlAttribute.Value.ToString());
        }
    }
}