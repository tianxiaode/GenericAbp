using System.Collections.Generic;
using JetBrains.Annotations;

namespace Generic.Abp.Metro.UI.Theme.Shared.Toolbars;

public class AbpToolbarOptions
{
    [NotNull]
    public List<IToolbarContributor> Contributors { get; }

    public AbpToolbarOptions()
    {
        Contributors = new List<IToolbarContributor>();
    }
}
