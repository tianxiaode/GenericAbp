using System.Collections.Generic;
using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;


public abstract class MetroBundleItemTagHelperService<TTagHelper, TService> : MetroTagHelperService<TTagHelper>
    where TTagHelper : MetroTagHelper<TTagHelper, TService>, IBundleItemTagHelper
    where TService : class, IMetroTagHelperService<TTagHelper>
{
    protected MetroTagHelperResourceService ResourceService { get; }

    protected MetroBundleItemTagHelperService(MetroTagHelperResourceService resourceService)
    {
        ResourceService = resourceService;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (context.Items.GetOrDefault(MetroTagHelperConsts.ContextBundleItemListKey) is List<BundleTagHelperItem> tagHelperItems)
        {
            output.SuppressOutput();
            tagHelperItems.Add(TagHelper.CreateBundleTagHelperItem());
        }
        else
        {
            await ResourceService.ProcessAsync(
                TagHelper.ViewContext,
                TagHelper,
                context,
                output,
                new List<BundleTagHelperItem>
                {
                        TagHelper.CreateBundleTagHelperItem()
                },
                TagHelper.GetNameOrNull()
            );
        }
    }
}
