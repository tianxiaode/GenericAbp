using Generic.Abp.Metro.UI.TagHelpers;
using Volo.Abp;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public abstract class MetroBundleItemTagHelper<TTagHelper, TTagHelperService> : MetroTagHelper<TTagHelper, TTagHelperService>, IBundleItemTagHelper
    where TTagHelper : MetroTagHelper<TTagHelper, TTagHelperService>, IBundleItemTagHelper
    where TTagHelperService : MetroBundleItemTagHelperService<TTagHelper, TTagHelperService>
{
    /// <summary>
    /// A file path.
    /// </summary>
    public string? Src { get; set; }

    /// <summary>
    /// A bundle contributor type.
    /// </summary>
    public Type? Type { get; set; } 

    protected MetroBundleItemTagHelper(TTagHelperService service)
        : base(service)
    {
    }

    public string GetNameOrNull()
    {
        if (Type != null)
        {
            return Type.FullName ?? string.Empty;
        }

        if (Src != null)
        {
            return Src
                .RemovePreFix("/")
                .RemovePostFix(StringComparison.OrdinalIgnoreCase, "." + GetFileExtension())
                .Replace("/", ".");
        }

        throw new AbpException("abp-script tag helper requires to set either src or type!");
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

        throw new AbpException("abp-script tag helper requires to set either src or type!");
    }

    protected abstract string GetFileExtension();
}
