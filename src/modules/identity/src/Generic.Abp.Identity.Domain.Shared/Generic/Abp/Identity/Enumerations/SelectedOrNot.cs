
using Generic.Abp.Enumeration;

namespace  Generic.Abp.Identity.Enumerations
{
    public class SelectedOrNot: Enumeration<SelectedOrNot>
    {
        public static readonly SelectedOrNot All = new (0, "All", isDefault:true, order:0);
        public static readonly SelectedOrNot Selected = new(1, "Selected", order:1);
        public static readonly SelectedOrNot Not = new(2 , "NotSelected", order:2);
        public SelectedOrNot(byte value, string name, string[] permission = null, bool isDefault = false, bool isPrivate = false, int order = 0) : base(value, name, permission, isDefault, isPrivate, order)
        {
        }
    }

    
}
