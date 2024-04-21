using System;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;

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
            var titleRow = sheet.CreateRow(0);
            for (var i = 0; i < orderedColumns.Count; i++)
            {
                var column = orderedColumns[i];
                var cell = titleRow.CreateCell(i);
                cell.SetCellValue(column.DisplayName);
                if (metadata is { ColumnHeaderStyle: not null })
                {
                    cell.CellStyle = 
                }
            }
        }

        // 写入数据
        var row = exportSchema != null && exportSchema.Template.HasColumnHeader ? 2 : 1;
        foreach (var record in data)
        {
            if (exportSchema != null)
            {
                for (var i = 0; i < orderedColumns.Count; i++)
                {
                    var column = orderedColumns[i];
                    var property = typeof(T).GetProperty(column.FieldName);
                    if (property == null) continue;
                    var value = property.GetValue(record);
                    var cell = worksheet.Cells[row, i + 1];
                    cell.Value = value;
                    if (column.Style != null)
                    {
                    }

                    ;
                }
            }
            else
            {
                worksheet.Cells.LoadFromCollection(new List<T> { record }, true);
            }

            row++;
        }

        return await package.GetAsByteArrayAsync(cancellationToken);
    }

    //static void SetFontStyle(ICellStyle style, JObject fontJson)
    //{
    //    IFont font = style.Workbook.CreateFont();
    //    font.Boldweight = fontJson["Bold"].Value<bool>() ? (short)FontBoldWeight.Bold : (short)FontBoldWeight.Normal;
    //    font.IsItalic = fontJson["Italic"].Value<bool>();
    //    font.Color = IndexedColors.Red.Index; // 这里需要根据 JSON 中的颜色值设置字体颜色
    //    style.SetFont(font);
    //}

    //static void SetFillStyle(ICellStyle style, JObject fillJson)
    //{
    //    style.FillPattern = Enum.Parse<FillPatternType>(fillJson["PatternType"].Value<string>());
    //    style.FillForegroundColor = IndexedColors.Yellow.Index; // 这里需要根据 JSON 中的颜色值设置填充颜色
    //}

    //static void SetBorderStyle(ICellStyle style, JObject borderJson)
    //{
    //    style.BorderTop = Enum.Parse<BorderStyle>(borderJson["Style"].Value<string>());
    //    style.BorderBottom = Enum.Parse<BorderStyle>(borderJson["Style"].Value<string>());
    //    style.BorderLeft = Enum.Parse<BorderStyle>(borderJson["Style"].Value<string>());
    //    style.BorderRight = Enum.Parse<BorderStyle>(borderJson["Style"].Value<string>());
    //    style.TopBorderColor = IndexedColors.Black.Index; // 这里需要根据 JSON 中的颜色值设置边框颜色
    //    style.BottomBorderColor = IndexedColors.Black.Index;
    //    style.LeftBorderColor = IndexedColors.Black.Index;
    //    style.RightBorderColor = IndexedColors.Black.Index;
    //}
}