using CsvHelper.Configuration;
using CsvHelper;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Abp.ExportManager;

public class ExcelExportService : IExportService
{
    public virtual async Task<byte[]> ExportAsync<T>(IEnumerable<T> data, CancellationToken cancellationToken = default)
    {
        using var package = new ExcelPackage();
        // 添加工作表
        var worksheet = package.Workbook.Worksheets.Add("Sheet1");

        // 写入表头
        var properties = typeof(T).GetProperties();
        for (int i = 0; i < properties.Length; i++)
        {
            worksheet.Cells[1, i + 1].Value = properties[i].Name;
        }

        // 写入数据
        int row = 2;
        foreach (var item in data)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                worksheet.Cells[row, i + 1].Value = properties[i].GetValue(item);
            }

            row++;
        }

        // 保存Excel文件到内存流
        using var memoryStream = new MemoryStream();
        package.SaveAs(memoryStream);
        return await memoryStream.GetAllBytesAsync(cancellationToken);
    }

    public byte[] GenerateExcel<T>(IEnumerable<T> data, ExcelClassMap<T> map)
    {
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            // 应用 ClassMap 中定义的表头和样式
            map.ConfigureWorksheet(worksheet);

            // 写入数据
            int row = 2;
            foreach (var item in data)
            {
                map.MapItem(worksheet, row, item);
                row++;
            }

            return package.GetAsByteArray();
        }
    }
}

public abstract class ExcelClassMap<T>
{
    public abstract void ConfigureWorksheet(ExcelWorksheet worksheet);
    public abstract void MapItem(ExcelWorksheet worksheet, int row, T item);
}