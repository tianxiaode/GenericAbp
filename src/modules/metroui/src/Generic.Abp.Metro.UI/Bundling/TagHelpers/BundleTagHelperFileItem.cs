using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public class BundleTagHelperFileItem : BundleTagHelperItem
{
    [NotNull]
    public string File { get; }

    public BundleTagHelperFileItem([NotNull] string file)
    {
        File = Check.NotNull(file, nameof(file));
    }

    public override string ToString()
    {
        return File;
    }

    public override void AddToConfiguration(BundleConfiguration configuration)
    {
        configuration.AddFiles(File);
    }
}
