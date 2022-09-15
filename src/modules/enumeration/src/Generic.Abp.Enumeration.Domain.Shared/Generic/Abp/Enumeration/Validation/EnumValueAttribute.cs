using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static System.Reflection.BindingFlags;

namespace Generic.Abp.Enumeration.Validation
{
    public class EnumValueAttribute : DataTypeAttribute
    {
        public Type Type { get; set; }


        public EnumValueAttribute(Type type) : base("Enumeration")
        {
            Type = type;
        }


        public override bool IsValid(object value)
        {
            return Type.GetFields(Public | Static).Any(m =>
            {
                var obj = m.GetValue(null);
                var enumValue = obj switch
                {
                    IEnumeration<int> enumeration => enumeration.Value,
                    IEnumeration<byte> enumeration => enumeration.Value,
                    _ => -1
                };

                return enumValue == Convert.ToInt32(value);

            });
        }

    }
}