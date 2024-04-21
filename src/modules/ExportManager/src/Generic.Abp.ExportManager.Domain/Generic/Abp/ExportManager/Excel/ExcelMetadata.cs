using System.Collections.Generic;
using Generic.Abp.ExportManager.Metadata;
using OfficeOpenXml.Style;

namespace Generic.Abp.ExportManager.Excel;

public class ExcelMetadata : IMetadata<ExcelColumn, ExcelStyle, ExcelStyle, ExcelStyle, ExcelStyle>
{
    public bool HasColumnHeader { get; set; } = true;
    public List<ExcelColumn> Columns { get; set; } = [];
    public string Title { get; set; }
    public string Description { get; set; }
    public string Footer { get; set; }
    public ExcelStyle TitleStyle { get; set; }
    public ExcelStyle ColumnHeaderStyle { get; set; }
    public ExcelStyle DescriptionStyle { get; set; }
    public ExcelStyle ColumnFooterStyle { get; set; }
}