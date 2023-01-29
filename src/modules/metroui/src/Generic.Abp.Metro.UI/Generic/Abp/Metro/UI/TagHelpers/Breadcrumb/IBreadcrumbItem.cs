namespace Generic.Abp.Metro.UI.TagHelpers.Breadcrumb;

public interface IBreadcrumbItem : IElementItem
{
    string Title { get; }
    string Cls { get; }
    string Url { get; }
}