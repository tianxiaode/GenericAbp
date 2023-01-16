namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public class AbpStyleTagHelperService : AbpBundleItemTagHelperService<AbpStyleTagHelper, AbpStyleTagHelperService>
{
    public AbpStyleTagHelperService(AbpTagHelperStyleService resourceService)
        : base(resourceService)
    {
    }
}
