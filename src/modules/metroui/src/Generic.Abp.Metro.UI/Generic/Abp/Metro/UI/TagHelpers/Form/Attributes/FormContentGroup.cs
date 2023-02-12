using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class FormContentGroup : Attribute
{
    public string GroupName { get; set; }

    public FormContentGroup(string groupName)
    {
        GroupName = groupName;
    }
}