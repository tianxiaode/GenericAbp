using System.Collections.Generic;
using Generic.Abp.ExportManager.Metadata;

namespace Generic.Abp.ExportManager.Csv;

public class CsvMetadata : IMetadata<DefaultColumn, DefaultTitleStyle, DefaultColumnHeaderStyle, DefaultDescriptionStyle
    ,
    DefaultFooterStyle>
{
    public bool HasColumnHeader { get; set; }
    public List<DefaultColumn> Columns { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Footer { get; set; }
    public DefaultTitleStyle TitleStyle { get; set; }
    public DefaultColumnHeaderStyle ColumnHeaderStyle { get; set; }
    public DefaultDescriptionStyle DescriptionStyle { get; set; }
    public DefaultFooterStyle ColumnFooterStyle { get; set; }

    public CsvMetadata()
    {
        Columns = [];
    }
}