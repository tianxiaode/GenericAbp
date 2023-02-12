using System;

namespace Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class TagInput : Attribute
{
    public int? MaxTags { get; set; }
    public bool RandomColor { get; set; } = false;
    public string TagSeparator { get; set; }
    public string ClsTag { get; set; }
    public string ClsTagTitle { get; set; }
    public string ClsTagRemover { get; set; }

    public TagInput()
    {
    }

    public TagInput(int maxTags)
    {
        MaxTags = maxTags;
    }
}