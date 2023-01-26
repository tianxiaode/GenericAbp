using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public abstract class MetroBundleTagHelper : TagHelper
{
    protected MetroBundleTagHelper(MetroTagHelperResourceService resourceService)
    {
        ResourceService = resourceService;
    }

    protected MetroTagHelperResourceService ResourceService { get; }
    [HtmlAttributeNotBound] [ViewContext] public ViewContext ViewContext { get; set; }

    public string Name { get; set; } = string.Empty;

    public virtual string GetNameOrNull()
    {
        return Name;
    }


    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await ResourceService.ProcessAsync(
            ViewContext,
            this,
            context,
            output,
            await GetBundleItems(context, output),
            GetNameOrNull()
        );
    }

    protected virtual async Task<List<BundleTagHelperItem>> GetBundleItems(TagHelperContext context,
        TagHelperOutput output)
    {
        var bundleItems = new List<BundleTagHelperItem>();
        context.Items[MetroTagHelperConsts.ContextBundleItemListKey] = bundleItems;
        var childContentAsync = await output.GetChildContentAsync();
        return bundleItems;
    }
}