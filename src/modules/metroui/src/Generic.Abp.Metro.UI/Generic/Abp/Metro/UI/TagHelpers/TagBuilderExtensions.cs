using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Generic.Abp.Metro.UI.TagHelpers;

internal static class TagBuilderExtensions
{
    public static string ToHtmlString(this TagBuilder tagBuilder)
    {
        return tagBuilder.ToHtmlString(HtmlEncoder.Default);
    }

    public static string ToHtmlString(this TagBuilder tagBuilder, HtmlEncoder htmlEncoder)
    {
        using var writer = new StringWriter();
        tagBuilder.WriteTo(writer, htmlEncoder);
        return writer.ToString();
    }
}
