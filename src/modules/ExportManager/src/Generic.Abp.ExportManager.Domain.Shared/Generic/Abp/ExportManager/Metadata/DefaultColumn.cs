namespace Generic.Abp.ExportManager.Metadata;

public abstract class DefaultColumn : IColumn
{
    public string FieldName { get; set; }
    public string DisplayName { get; set; }
    public int Order { get; set; }
}