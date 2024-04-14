using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace Generic.Abp.ExportManager;

public class CsvExportService : IExportService
{
    public async Task<byte[]> ExportAsync<T>(IEnumerable<T> data, CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream);
        using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
        await csv.WriteRecordsAsync(data, cancellationToken);
        await writer.FlushAsync();
        return await memoryStream.GetAllBytesAsync(cancellationToken);
    }

    public virtual async Task<byte[]> ExportCsv<T>(IEnumerable<T> data, ClassMap<T> map,
        CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream);
        await using var csv =
            new CsvWriter(writer, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture));
        csv.Context.RegisterClassMap(map); // 注册 ClassMap
        await csv.WriteRecordsAsync(data, cancellationToken);
        await writer.FlushAsync();
        return await memoryStream.GetAllBytesAsync(cancellationToken);
    }
}