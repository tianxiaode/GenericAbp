using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Extensions;

public static class TagHelperOutputExtensions
{
    public static Task<string> RenderAsync(this TagHelperOutput output, HtmlEncoder htmlEncoder)
    {
        using var writer = new StringWriter();
        output.WriteTo(writer, htmlEncoder);
        return Task.FromResult(writer.ToString());
    }
}
