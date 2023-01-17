namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public class MetroMetroBundleTagHelperService : MetroBundleTagHelperService<MetroScriptBundleTagHelper, MetroMetroBundleTagHelperService>
{
    public MetroMetroBundleTagHelperService(MetroTagHelperScriptService resourceHelper)
        : base(resourceHelper)
    {
    }
}
