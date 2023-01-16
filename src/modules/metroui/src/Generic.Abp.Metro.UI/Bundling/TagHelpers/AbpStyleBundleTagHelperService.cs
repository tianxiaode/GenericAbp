namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public class AbpStyleBundleTagHelperService : AbpBundleTagHelperService<AbpStyleBundleTagHelper, AbpStyleBundleTagHelperService>
{
    public AbpStyleBundleTagHelperService(AbpTagHelperStyleService resourceHelper)
        : base(resourceHelper)
    {
    }
}
