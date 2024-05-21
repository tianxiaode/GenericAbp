using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Abp.ExportManager.Excel;

public class ExcelExportService : IExportService<ExcelMetadata>
{
    public virtual async Task<byte[]> ExportAsync<T>(IEnumerable<T> data, ExcelMetadata metadata = null,
        CancellationToken cancellationToken = default)
    {
        var workbook = new XSSFWorkbook();
        var sheetTitle = metadata is { Title: not null }
            ? metadata.Title
            : "Sheet1";
        var sheet = workbook.CreateSheet(sheetTitle);
        var hasHeader = metadata is { HasColumnHeader: true };
        var orderedColumns =
            hasHeader ? metadata.Columns.OrderBy(c => c.Order).ToList() : [];

        // 写入列标题
        if (hasHeader)
        {
            await WriteHeaderAsync(sheet, metadata, orderedColumns);
        }

        // 写入数据
        var row = hasHeader ? 2 : 1;
        await WriteDataAsync(sheet, row, data.ToList(), metadata, orderedColumns);

        using var stream = new MemoryStream();
        workbook.Write(stream);

        return await stream.GetAllBytesAsync(cancellationToken);
    }

    protected virtual async Task WriteHeaderAsync(ISheet sheet, ExcelMetadata metadata, List<ExcelColumn> columns)
    {
        var titleRow = sheet.CreateRow(0);
        if (metadata.RowHeight.HasValue) titleRow.Height = metadata.RowHeight.Value;
        for (var i = 0; i < columns.Count; i++)
        {
            var column = columns[i];
            var cell = titleRow.CreateCell(i);
            cell.SetCellValue(column.DisplayName);
            if (column.IsAutoSize) sheet.AutoSizeColumn(i);
            if (column is { HeaderCellStyle: not null })
            {
                cell.CellStyle = await SetCellStyleAsync(sheet.Workbook, column.HeaderCellStyle);
            }
        }
    }

    protected virtual async Task WriteDataAsync<T>(ISheet sheet, int rowIndex, List<T> data,
        ExcelMetadata metadata,
        List<ExcelColumn> columns)
    {
        if (metadata == null)
        {
            foreach (var item in data)
            {
                var row = sheet.CreateRow(rowIndex++);
                var properties = typeof(T).GetProperties();
                var cellIndex = 0;
                foreach (var property in properties)
                {
                    var value = property.GetValue(item);
                    var cell = row.CreateCell(cellIndex++);
                    await SetCellValueAsync(cell, property, value);
                }
            }

            return;
        }

        foreach (var record in data)
        {
            var row = sheet.CreateRow(rowIndex);
            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];
                var property = typeof(T).GetProperty(column.FieldName);
                if (property == null) continue;
                var value = property.GetValue(record);
                var cell = row.CreateCell(i);
                await SetCellValueAsync(cell, property, value);
                if (column.CellStyle != null)
                {
                    await SetCellStyleAsync(sheet.Workbook, column.CellStyle);
                }
            }

            rowIndex++;
        }
    }

    protected virtual async Task<ICellStyle> SetCellStyleAsync(IWorkbook workbook, ExcelCellStyle excelCellStyle)
    {
        var style = workbook.CreateCellStyle();
        await SetCellFontStyleAsync(workbook, style, excelCellStyle);
        await SetFillStyleAsync(style, excelCellStyle);
        await SetBorderStyleAsync(style, excelCellStyle);

        style.Alignment = excelCellStyle.Alignment;
        style.VerticalAlignment = excelCellStyle.VerticalAlignment;
        style.DataFormat = excelCellStyle.DataFormat;
        style.Rotation = excelCellStyle.Rotation;
        style.IsLocked = excelCellStyle.IsLocked;
        style.WrapText = excelCellStyle.WrapText;
        style.Indention = excelCellStyle.Indention;
        return style;
    }

    protected virtual Task SetCellFontStyleAsync(IWorkbook workbook, ICellStyle style, ExcelCellStyle cellStyle)
    {
        var font = workbook.CreateFont();
        var excelFont = cellStyle.Font;
        font.IsStrikeout = excelFont.IsStrikeout;
        font.FontName = excelFont.FontName;
        font.FontHeight = excelFont.FontHeight;
        font.FontHeightInPoints = excelFont.FontHeightInPoints;
        font.IsBold = excelFont.IsBold;
        font.IsItalic = excelFont.IsItalic;
        font.Color = excelFont.Color;
        font.Boldweight = excelFont.BoldWeight;
        font.Underline = excelFont.Underline;
        font.TypeOffset = excelFont.TypeOffset;
        font.Charset = excelFont.Charset;
        style.SetFont(font);
        return Task.CompletedTask;
    }

    protected virtual Task SetFillStyleAsync(ICellStyle style, ExcelCellStyle cellStyle)
    {
        style.FillPattern = cellStyle.FillPattern;
        style.FillBackgroundColor = cellStyle.FillBackgroundColor;
        style.FillForegroundColor = cellStyle.FillForegroundColor;
        return Task.CompletedTask;
    }

    protected virtual Task SetBorderStyleAsync(ICellStyle style, ExcelCellStyle cellStyle)
    {
        style.BorderTop = cellStyle.BorderTop;
        style.BorderBottom = cellStyle.BorderBottom;
        style.BorderLeft = cellStyle.BorderLeft;
        style.BorderRight = cellStyle.BorderRight;
        style.TopBorderColor = cellStyle.TopBorderColor;
        style.BottomBorderColor = cellStyle.BottomBorderColor;
        style.LeftBorderColor = cellStyle.LeftBorderColor;
        style.RightBorderColor = cellStyle.RightBorderColor;
        style.BorderDiagonalColor = cellStyle.BorderDiagonalColor;
        style.BorderDiagonalLineStyle = cellStyle.BorderDiagonalLineStyle;
        style.BorderDiagonal = cellStyle.BorderDiagonal;
        return Task.CompletedTask;
    }

    protected virtual Task SetCellValueAsync(ICell cell, PropertyInfo property, object value)
    {
        if (value == null) return Task.CompletedTask;
        if (property.PropertyType == typeof(string))
        {
            cell.SetCellValue((string)value);
        }
        else if (property.PropertyType == typeof(int))
        {
            cell.SetCellValue((int)value);
        }
        else if (property.PropertyType == typeof(double))
        {
            cell.SetCellValue((double)value);
        }
        else if (property.PropertyType == typeof(DateTime))
        {
            var dateValue = (DateTime)value;
            cell.SetCellValue(dateValue.ToOADate());
        }
        else
        {
        }

        return Task.CompletedTask;
    }
}