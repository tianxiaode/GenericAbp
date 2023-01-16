namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public class MetroScriptTagHelperService : MetroBundleItemTagHelperService<MetroScriptTagHelper, MetroScriptTagHelperService>
{
    public MetroScriptTagHelperService(MetroTagHelperScriptService resourceService)
        : base(resourceService)
    {
    }
}
