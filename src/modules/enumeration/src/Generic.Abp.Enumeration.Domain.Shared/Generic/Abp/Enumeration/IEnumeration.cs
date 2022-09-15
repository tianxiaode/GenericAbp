using System;

namespace Generic.Abp.Enumeration
{
    public interface IEnumeration<out TValue> 
    {
        string Name { get; }
        TValue Value { get; }
        string[] Permission { get; }
        bool IsDefault { get; }
        bool IsPrivate { get; }
        string ResourceName { get; }
        int Order { get; }
        Type GetValueType();

        
    }
}
