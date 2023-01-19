namespace Generic.Abp.Metro.UI.TagHelpers.Dialog;

public class MetroDialogTitleTagHelper : MetroTagHelper<MetroDialogTitleTagHelper, MetroDialogTitleTagHelperService>
{
    public string Title { get; set; }

    public MetroDialogTitleTagHelper(MetroDialogTitleTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
