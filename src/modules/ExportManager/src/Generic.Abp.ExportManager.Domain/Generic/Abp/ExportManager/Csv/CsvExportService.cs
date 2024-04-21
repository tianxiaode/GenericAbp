using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace Generic.Abp.ExportManager.Csv;

public class CsvExportService : IExportService<CsvMetadata>
{
    public async Task<byte[]> ExportAsync<T>(IEnumerable<T> data, CsvMetadata metadata = null,
        CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream();
        await using var writer = new StreamWriter(memoryStream);
        await using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));

        // 如果有 metadata 并且设置了 HasColumnHeader，写入列标题
        if (metadata is { HasColumnHeader: true })
        {
            foreach (var column in metadata.Columns.OrderBy(c => c.Order))
            {
                csv.WriteField(column.FieldName);
            }

            await csv.NextRecordAsync();
        }

        // 写入数据
        foreach (var record in data)
        {
            if (metadata != null)
            {
                var orderedColumns = metadata.Columns.OrderBy(c => c.Order);
                foreach (var column in orderedColumns)
                {
                    var property = typeof(T).GetProperty(column.FieldName);
                    if (property == null) continue;
                    var value = property.GetValue(record);
                    csv.WriteField(value);
                }
            }
            else
            {
                csv.WriteRecord(record);
            }

            await csv.NextRecordAsync();
        }

        await writer.FlushAsync();
        return await memoryStream.GetAllBytesAsync(cancellationToken);
    }
}