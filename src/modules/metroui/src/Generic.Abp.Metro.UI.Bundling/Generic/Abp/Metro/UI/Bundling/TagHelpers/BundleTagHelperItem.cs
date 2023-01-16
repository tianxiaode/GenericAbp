using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public abstract class BundleTagHelperItem
{
    public abstract void AddToConfiguration(BundleConfiguration configuration);
}
