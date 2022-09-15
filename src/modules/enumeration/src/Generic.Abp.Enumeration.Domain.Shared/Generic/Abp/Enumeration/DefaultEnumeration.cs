namespace Generic.Abp.Enumeration
{
    public class DefaultEnumeration : Enumeration<DefaultEnumeration>
    {
        protected DefaultEnumeration(byte value, string name, string[] permission = null, bool isDefault = false,
            bool isPrivate = false) : base(value, name, permission, isDefault, isPrivate)
        {
        }
    }
}
