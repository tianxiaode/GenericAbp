namespace Generic.Abp.Enumeration
{
    public class DefaultEnumeration : Enumeration
    {
        protected DefaultEnumeration(int id, string name, string[] permission = null, bool isDefault = false,
            bool isPrivate = false) : base(id, name, permission, isDefault, isPrivate)
        {
        }
    }
}