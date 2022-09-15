using System;

namespace Generic.Abp.Enumeration
{
    [Serializable]
    public class EnumDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public int Value { get; set; }
        public bool IsDefault { get; set; }
        public string ResourceName { get; set; }
        public int Order { get; set; }

        public EnumDto(string id, string name, string text, int value, bool isDefault, string resourceName, int order)
        {
            Id = id;
            Name = name;
            Text = text;
            Value = value;
            IsDefault = isDefault;
            ResourceName = resourceName;
            Order = order;
        }
    }
}
