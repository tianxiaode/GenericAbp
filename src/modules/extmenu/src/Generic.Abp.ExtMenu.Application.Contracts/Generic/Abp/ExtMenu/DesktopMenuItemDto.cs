using System.Collections.Generic;
using System.ComponentModel;

namespace Generic.Abp.ExtMenu
{
    public class DesktopMenuItemDto : MenuItemBaseDto
    {
        public int? ParentId { get; set; }

        public IEnumerable<DesktopMenuItemDto> Children { get; set; }

        [DefaultValue(true)]
        public bool Selectable { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [DefaultValue(true)]
        public bool Leaf { get; set; }

    }
}
