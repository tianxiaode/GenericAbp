namespace Generic.Abp.Metro.UI.TagHelpers.Collapse;

public class AbpAccordionTagHelper : AbpTagHelper<AbpAccordionTagHelper, AbpAccordionTagHelperService>
{
    public string Id { get; set; }

    public AbpAccordionTagHelper(AbpAccordionTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
