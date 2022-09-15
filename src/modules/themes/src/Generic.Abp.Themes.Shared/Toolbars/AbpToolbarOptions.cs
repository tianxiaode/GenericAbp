using JetBrains.Annotations;
using System.Collections.Generic;

namespace Generic.Abp.Themes.Shared.Toolbars
{
    public class AbpToolbarOptions
    {
        [NotNull]
        public List<IToolbarContributor> Contributors { get; }

        public AbpToolbarOptions()
        {
            Contributors = new List<IToolbarContributor>();
        }
    }
}
