using System.Collections.Generic;
using System.ComponentModel;

namespace Generic.Abp.ExtMenu
{
    public class DesktopMenuItemDto : MenuItemDto
    {
        public int? ParentId { get; set; }

        public List<DesktopMenuItemDto> Children { get; set; }

        public bool Selectable { get; set; }

        public bool Leaf { get; set; }

    }
}
