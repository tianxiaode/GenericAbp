using System.Collections.Generic;
using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public abstract class MetroBundleTagHelperService<TTagHelper, TService> : MetroTagHelperService<TTagHelper>
    where TTagHelper : MetroTagHelper<TTagHelper, TService>, IBundleTagHelper
    where TService : class, IMetroTagHelperService<TTagHelper>
{
    protected MetroTagHelperResourceService ResourceService { get; }

    protected MetroBundleTagHelperService(MetroTagHelperResourceService resourceService)
    {
        ResourceService = resourceService;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await ResourceService.ProcessAsync(
            TagHelper.ViewContext,
            TagHelper,
            context,
            output,
            await GetBundleItems(context, output),
            TagHelper.GetNameOrNull()
        );
    }

    protected virtual async Task<List<BundleTagHelperItem>> GetBundleItems(TagHelperContext context, TagHelperOutput output)
    {
        var bundleItems = new List<BundleTagHelperItem>();
        context.Items[MetroTagHelperConsts.ContextBundleItemListKey] = bundleItems;
        await output.GetChildContentAsync(); //TODO: Is there a way of executing children without getting content?
        return bundleItems;
    }
}
