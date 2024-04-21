namespace Generic.Abp.ExportManager.Metadata;

public interface IColumn
{
    string FieldName { get; set; }
    string DisplayName { get; set; }
    int Order { get; set; }
}