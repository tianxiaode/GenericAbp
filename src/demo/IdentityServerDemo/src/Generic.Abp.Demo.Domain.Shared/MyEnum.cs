namespace Generic.Abp.Demo
{
    class MyEnum:  Generic.Abp.Enumeration.Enumeration<MyEnum>
    {
        public static readonly MyEnum MyEnum1 = new MyEnum(1, "MyEnum1", isDefault: true);
        public static readonly MyEnum MyEnum2 = new MyEnum(2, "MyEnum2");

        protected MyEnum(byte value, string name, string[] permission = null, bool isDefault = false,
            bool isPrivate = false) : base(value, name, permission, isDefault, isPrivate)
        {
        }
    }
}
