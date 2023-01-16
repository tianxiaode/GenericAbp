using Generic.Abp.Metro.UI.TagHelpers;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public abstract class MetroBundleTagHelper<TTagHelper, TService> : MetroTagHelper<TTagHelper, TService>, IBundleTagHelper
    where TTagHelper : MetroTagHelper<TTagHelper, TService>
    where TService : class, IMetroTagHelperService<TTagHelper>
{
    public string Name { get; set; } = string.Empty;

    protected MetroBundleTagHelper(TService service)
        : base(service)
    {

    }

    public virtual string GetNameOrNull()
    {
        return Name;
    }
}
