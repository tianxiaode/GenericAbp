namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public class AbpScriptBundleTagHelperService : AbpBundleTagHelperService<AbpScriptBundleTagHelper, AbpScriptBundleTagHelperService>
{
    public AbpScriptBundleTagHelperService(AbpTagHelperScriptService resourceHelper)
        : base(resourceHelper)
    {
    }
}
