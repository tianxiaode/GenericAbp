namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public class MetroStyleTagHelperService : MetroBundleItemTagHelperService<MetroStyleTagHelper, MetroStyleTagHelperService>
{
    public MetroStyleTagHelperService(MetroTagHelperStyleService resourceService)
        : base(resourceService)
    {
    }
}
