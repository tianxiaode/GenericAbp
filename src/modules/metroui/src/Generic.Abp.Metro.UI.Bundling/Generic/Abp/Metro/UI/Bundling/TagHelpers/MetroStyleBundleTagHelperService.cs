namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public class MetroStyleBundleTagHelperService : MetroBundleTagHelperService<MetroStyleBundleTagHelper, MetroStyleBundleTagHelperService>
{
    public MetroStyleBundleTagHelperService(MetroTagHelperStyleService resourceHelper)
        : base(resourceHelper)
    {
    }
}
