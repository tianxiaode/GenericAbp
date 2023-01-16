using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public class BundleTagHelperContributorTypeItem : BundleTagHelperItem
{
    [NotNull]
    public Type Type { get; }

    public BundleTagHelperContributorTypeItem([NotNull] Type type)
    {
        Type = Check.NotNull(type, nameof(type));
    }

    public override string ToString()
    {
        return Type.FullName ?? string.Empty;
    }

    public override void AddToConfiguration(BundleConfiguration configuration)
    {
        configuration.AddContributors(Type);
    }
}
