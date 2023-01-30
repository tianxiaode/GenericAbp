namespace Generic.Abp.Metro.UI.TagHelpers.Breadcrumb;

public interface IBreadcrumbItem : IHasDisplayOrder
{
    string Title { get; }
    string Cls { get; }
    string Url { get; }
}