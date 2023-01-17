using Volo.Abp.AspNetCore.Mvc.UI.Alerts;

namespace Generic.Abp.Metro.UI.TagHelpers.Alert;

public class MetroAlertTagHelper : MetroTagHelper<MetroAlertTagHelper, MetroAlertTagHelperService>
{
    public AlertType AlertType { get; set; } = AlertType.Default;

    public bool? Dismissible { get; set; }

    public MetroAlertTagHelper(MetroAlertTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
