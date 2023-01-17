using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Extensions;

public static class TagHelperContextExtensions
{
    public static T GetValue<T>(this TagHelperContext context, string key)
    {
        if (!context.Items.ContainsKey(key))
        {
            return default(T);
        }

        return (T)context.Items[key];
    }
}
