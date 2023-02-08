using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class DisplayOrder : Attribute
{
    public static int Default = 10000;

    public int Number { get; set; }

    public DisplayOrder(int number)
    {
        Number = number;
    }
}