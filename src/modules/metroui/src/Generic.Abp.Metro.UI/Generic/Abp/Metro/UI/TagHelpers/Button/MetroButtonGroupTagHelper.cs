namespace Generic.Abp.Metro.UI.TagHelpers.Button;

public class MetroButtonGroupTagHelper : MetroTagHelper<MetroButtonGroupTagHelper, MetroButtonGroupTagHelperService>
{
    public bool Multi { get; set; } = true;
    public MetroButtonGroupTagHelper(MetroButtonGroupTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
