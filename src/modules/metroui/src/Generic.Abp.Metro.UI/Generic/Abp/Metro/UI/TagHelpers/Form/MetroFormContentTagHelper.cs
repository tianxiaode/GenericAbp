using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroFormContentTagHelper : MetroTagHelper<MetroFormContentTagHelper, MetroFormContentTagHelperService>, ITransientDependency
{
    public int Cols { get; set; } = 1;
    public bool Horizontal { get; set; } = false;
    public int LabelWidth { get; set; } = 100;
    public MetroFormContentTagHelper(MetroFormContentTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
