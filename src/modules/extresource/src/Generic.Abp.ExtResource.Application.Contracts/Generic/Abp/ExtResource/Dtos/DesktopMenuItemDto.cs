using System.Collections.Generic;

namespace Generic.Abp.ExtResource.Dtos
{
    public class DesktopMenuItemDto : MenuItemDto
    {
        public int? ParentId { get; set; }

        public List<DesktopMenuItemDto> Children { get; set; }

        public bool Selectable { get; set; }

        public bool Leaf { get; set; }

    }
}
