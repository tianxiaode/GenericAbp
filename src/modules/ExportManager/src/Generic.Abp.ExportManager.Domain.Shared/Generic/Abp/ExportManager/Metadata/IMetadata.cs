using System.Collections.Generic;
using JetBrains.Annotations;

namespace Generic.Abp.ExportManager.Metadata;

public interface IMetadata<TColumn, TTitleStyle, TDescriptionStyle, TFooterStyle>
    where TColumn : IColumn
    where TDescriptionStyle : class
    where TTitleStyle : class
    where TFooterStyle : class
{
    bool HasColumnHeader { get; set; }
    [NotNull] List<TColumn> Columns { get; set; }
    [CanBeNull] string Title { get; set; }
    [CanBeNull] string Description { get; set; }
    [CanBeNull] string Footer { get; set; }
    [CanBeNull] TTitleStyle TitleStyle { get; set; }
    [CanBeNull] TDescriptionStyle DescriptionStyle { get; set; }
    [CanBeNull] TFooterStyle ColumnFooterStyle { get; set; }
}