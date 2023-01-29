namespace Generic.Abp.Metro.UI.TagHelpers.Breadcrumb;

public class MetroBreadcrumbItem : IBreadcrumbItem
{
    public string Title { get; set; }
    public string Cls { get; set; }
    public string Url { get; set; }
    public int DisplayOrder { get; set; } = 0;
}