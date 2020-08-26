using System;
using System.Collections.Generic;

namespace Generic.Abp.Enumeration
{
    public class EnumerationOptions
    {
        public List<Type> Resources { get; }

        public EnumerationOptions()
        {
            Resources = new List<Type>();
        }

    }
}