using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public interface IMetroTagHelperResourceService : ITransientDependency
{
    Task ProcessAsync(
        ViewContext viewContext,
        TagHelper tagHelper,
        TagHelperContext context,
        TagHelperOutput output,
        List<BundleTagHelperItem> bundleItems,
        string? bundleName = null);
}