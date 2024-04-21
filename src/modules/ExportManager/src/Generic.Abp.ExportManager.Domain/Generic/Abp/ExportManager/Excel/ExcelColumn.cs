using Generic.Abp.ExportManager.Metadata;
using OfficeOpenXml.Style;

namespace Generic.Abp.ExportManager.Excel;

public class ExcelColumn : DefaultColumn
{
    public ExcelStyle Style { get; set; }
}