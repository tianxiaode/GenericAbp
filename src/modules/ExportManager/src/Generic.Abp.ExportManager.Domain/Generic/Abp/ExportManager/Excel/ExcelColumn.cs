using Generic.Abp.ExportManager.Metadata;

namespace Generic.Abp.ExportManager.Excel;

public class ExcelColumn : DefaultColumn
{
    public bool IsAutoSize { get; set; } = true;
    public ExcelCellStyle CellStyle { get; set; }

    public ExcelCellStyle HeaderCellStyle { get; set; }
}