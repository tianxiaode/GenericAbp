using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static System.Reflection.BindingFlags;

namespace Generic.Abp.Enumeration.Validation
{
    public class EnumValueRangeAttribute : DataTypeAttribute
    {
        public string[] ValueList { get; set; }
        public Type Type { get; set; }



        public EnumValueRangeAttribute(string[] valueList, Type type) : base("Enumeration")
        {
            Type = type;
            ValueList = valueList;
        }


        public override bool IsValid(object value)
        {
            return Type.GetFields(Public | Static).Any(m =>
            {
                var enumeration = (Enumeration)m.GetValue(null);
                return enumeration != null && enumeration.Id == (int) value && ValueList.Contains(m.Name);

            });
        }

    }
}