using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.Button;

public class MetroLinkButtonTagHelperService : MetroButtonTagHelperServiceBase<MetroLinkButtonTagHelper>
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await base.ProcessAsync(context, output);
        await AddTypeAsync(context, output);
        await AddRoleAsync(context, output);
    }

    protected virtual Task AddTypeAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (!output.Attributes.ContainsName("type") &&
            output.TagName.Equals("input", StringComparison.InvariantCultureIgnoreCase))
        {
            output.Attributes.Add("type", "button");
        }
        return Task.CompletedTask;
    }

    protected virtual Task AddRoleAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (!output.Attributes.ContainsName("role"))
        {
            output.Attributes.Add("role", "button");
        }
        return Task.CompletedTask;
    }
}
