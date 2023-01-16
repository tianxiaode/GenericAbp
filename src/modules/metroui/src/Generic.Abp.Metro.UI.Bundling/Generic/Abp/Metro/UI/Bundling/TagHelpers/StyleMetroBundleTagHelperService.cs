namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public class StyleMetroBundleTagHelperService : MetroBundleTagHelperService<StyleMetroBundleTagHelper, StyleMetroBundleTagHelperService>
{
    public StyleMetroBundleTagHelperService(MetroTagHelperStyleService resourceHelper)
        : base(resourceHelper)
    {
    }
}
