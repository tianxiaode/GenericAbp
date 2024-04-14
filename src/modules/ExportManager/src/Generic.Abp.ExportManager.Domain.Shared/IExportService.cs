using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Abp.ExportManager;

public interface IExportService
{
    Task<byte[]> ExportAsync<T>(IEnumerable<T> data, CancellationToken cancellationToken = default);
}