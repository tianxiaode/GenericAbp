using System;
using System.Collections.Generic;

namespace Generic.Abp.ExtResource.Dtos
{
    public class DesktopMenuItemDto : MenuItemDto
    {
        public Guid? ParentId { get; set; }

        public List<DesktopMenuItemDto> Children { get; set; }

        public bool Selectable { get; set; }

        public bool Leaf { get; set; }


        public DesktopMenuItemDto(Guid id, string langText, string iconCls, string viewType, bool selectable) : base(id, langText, iconCls, viewType)
        {
            ParentId = null;
            Selectable = selectable;
            Leaf = true;
            Children = null;
        }

        public DesktopMenuItemDto(Guid id, string langText, string iconCls, string viewType, bool selectable, Guid? parentId) : base(id, langText, iconCls, viewType)
        {
            Selectable = selectable;
            ParentId = parentId;
            Leaf = true;
            Children = null;
        }
    }
}
