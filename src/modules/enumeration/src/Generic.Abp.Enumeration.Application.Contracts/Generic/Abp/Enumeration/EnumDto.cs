using System;

namespace Generic.Abp.Enumeration
{
    [Serializable]
    public class EnumDto
    {
        public string Id { get; set; }
        public string Type { get; set; }

        public string Text { get; set; }

        public int Value { get; set; }

        public bool IsDefault { get; set; }

        public string Key { get; set; }

    }
}
