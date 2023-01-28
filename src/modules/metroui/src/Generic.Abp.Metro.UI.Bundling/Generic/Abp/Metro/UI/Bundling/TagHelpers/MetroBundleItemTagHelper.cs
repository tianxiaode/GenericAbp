using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public abstract class MetroBundleItemTagHelper : MetroTagHelper
{
    protected MetroBundleItemTagHelper(MetroTagHelperResourceService resourceService)
    {
        ResourceService = resourceService;
    }

    protected MetroTagHelperResourceService ResourceService { get; }

    public string? Src { get; set; }

    public Type? Type { get; set; }


    public string? GetNameOrNull()
    {
        if (Type != null)
        {
            return Type.FullName;
        }

        if (Src != null)
        {
            return Src
                .RemovePreFix("/")
                .RemovePostFix(StringComparison.OrdinalIgnoreCase, "." + GetFileExtension())
                .Replace("/", ".");
        }

        throw new AbpException("abp-script tag helper requires to set either src or color!");
    }

    public BundleTagHelperItem CreateBundleTagHelperItem()
    {
        if (Type != null)
        {
            return new BundleTagHelperContributorTypeItem(Type);
        }

        if (Src != null)
        {
            return new BundleTagHelperFileItem(Src);
        }

        throw new AbpException("abp-script tag helper requires to set either src or color!");
    }

    protected abstract string GetFileExtension();

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (context.Items.GetOrDefault(MetroTagHelperConsts.ContextBundleItemListKey) is List<BundleTagHelperItem>
            tagHelperItems)
        {
            output.SuppressOutput();
            tagHelperItems.Add(CreateBundleTagHelperItem());
        }
        else
        {
            await ResourceService.ProcessAsync(
                ViewContext,
                this,
                context,
                output,
                new List<BundleTagHelperItem>
                {
                    CreateBundleTagHelperItem()
                },
                GetNameOrNull()
            );
        }
    }
}