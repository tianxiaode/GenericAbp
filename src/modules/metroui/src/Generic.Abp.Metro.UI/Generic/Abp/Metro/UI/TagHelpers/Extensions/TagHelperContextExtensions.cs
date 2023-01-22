using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Extensions;

public static class TagHelperContextExtensions
{
    public static  T GetValue<T>(this TagHelperContext context, string key)
    {
        return !context.Items.ContainsKey(key) ? default(T) : (T)context.Items[key];
    }
}
