using System;

namespace Generic.Abp.Enumeration
{
    public interface IEnumeration : IComparable
    {
        string Name { get; }
        int Id { get; }
        string[] Permission { get; }
        bool IsDefault { get; }

        bool IsPrivate { get; }
        string ToString();
        bool Equals(object obj);
        int GetHashCode();
        
    }
}