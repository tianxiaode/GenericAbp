using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static System.Reflection.BindingFlags;

namespace Generic.Abp.Enumeration.Validation
{
    public class EnumValueAttribute : DataTypeAttribute
    {
        public Type Type { get; set; }

        // public string GetErrorMessage() =>
        //     "The field {0} is invalid.";

        public EnumValueAttribute(Type type) : base("Enumeration")
        {
            Type = type;
        }


        public override bool IsValid(object value)
        {
            return Type.GetFields(Public | Static).Any(m =>
            {
                var enumeration = (Enumeration)m.GetValue(null);
                return enumeration.Id == Convert.ToInt32(value);

            });

            //return Enum.IsDefined(Type,value);
        }

    }
}