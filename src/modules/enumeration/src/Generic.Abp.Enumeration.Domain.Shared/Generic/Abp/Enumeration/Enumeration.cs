using Ardalis.SmartEnum;
using JetBrains.Annotations;
using System;


namespace Generic.Abp.Enumeration
{

    public abstract class Enumeration<TEnum> : Enumeration<TEnum,byte>
        where TEnum : SmartEnum<TEnum, byte>
    {
        protected Enumeration(byte value, string name, string[] permission = null, bool isDefault = false,bool isPrivate = false, int order = 0) : base(value, name, permission, isDefault,isPrivate, order)
        {
        }
    }

    public abstract  class Enumeration<TEnum,TValue> : SmartEnum<TEnum, TValue> , IEnumeration<TValue>
        where TEnum : SmartEnum<TEnum, TValue>
        where TValue : IEquatable<TValue>, IComparable<TValue>
    {
        protected Enumeration(TValue value, string name , string[] permission = null, bool isDefault = false,bool isPrivate = false, int order =0) : base(name, value)
        {
            IsDefault = isDefault;
            IsPrivate = IsPrivate;
            Order = order;
            Permission = permission;
        }

        [CanBeNull]
        public string[] Permission { get; }
        public int Order { get; }
        public bool IsDefault { get; }
        public bool IsPrivate { get; set; }
        public string ResourceName { get; set; }

    }

}
