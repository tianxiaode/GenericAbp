using JetBrains.Annotations;

namespace Generic.Abp.Metro.UI.Theme.Shared.Toolbars;

public class MetroToolbarOptions
{
    [NotNull]
    public List<IToolbarContributor> Contributors { get; }

    public MetroToolbarOptions()
    {
        Contributors = new List<IToolbarContributor>();
    }
}
