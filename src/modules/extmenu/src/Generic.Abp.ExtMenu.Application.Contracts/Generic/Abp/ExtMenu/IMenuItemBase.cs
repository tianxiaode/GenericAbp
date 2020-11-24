using System;

namespace Generic.Abp.ExtMenu
{
    public interface IMenuItemBase
    {
        int Id { get; set; }
        string LangText { get; set; }
        string IconCls { get; set; }
        string ViewType { get; set; }

    }
}
