using JetBrains.Annotations;
using System.Collections.Generic;
using Volo.Abp;

namespace Generic.Abp.Themes.Shared.Toolbars
{
    public class Toolbar
    {
        public string Name { get; }

        public List<ToolbarItem> Items { get; }

        public Toolbar([NotNull] string name)
        {
            Name = Check.NotNull(name, nameof(name));
            Items = new List<ToolbarItem>();
        }
    }
}
