using System.Collections.Generic;
using Generic.Abp.ExportManager.Metadata;

namespace Generic.Abp.ExportManager.Excel;

public class ExcelMetadata : IMetadata<ExcelColumn, ExcelCellStyle, ExcelCellStyle, ExcelCellStyle>
{
    public bool HasColumnHeader { get; set; } = true;
    public List<ExcelColumn> Columns { get; set; } = [];
    public string Title { get; set; }
    public string Description { get; set; }
    public string Footer { get; set; }
    public short? RowHeight { get; set; }
    public ExcelCellStyle TitleStyle { get; set; }
    public ExcelCellStyle DescriptionStyle { get; set; }
    public ExcelCellStyle ColumnFooterStyle { get; set; }
}