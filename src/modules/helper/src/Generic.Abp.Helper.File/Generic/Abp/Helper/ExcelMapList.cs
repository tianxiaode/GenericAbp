namespace Generic.Abp.Helper
{
    public class ExcelMapList
    {
        public int Column { get; set; }

        public string PropertyName { get; set; }

        public object Value { get; set; }

        public bool IsId { get; set; }

        public ExcelMapList()
        {
        }

        public ExcelMapList(int column, string propertyName, bool isId = false, object value = null)
        {
            Column = column;
            PropertyName = propertyName;
            IsId = isId;
            Value = value;
        }
    }
}
