namespace Generic.Abp.Metro.UI.TagHelpers.Collapse;

public class AbpCollapseBodyTagHelper : AbpTagHelper<AbpCollapseBodyTagHelper, AbpCollapseBodyTagHelperService>
{
    public string Id { get; set; }

    public bool? Multi { get; set; }

    public bool? Show { get; set; }

    public AbpCollapseBodyTagHelper(AbpCollapseBodyTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
