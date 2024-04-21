using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Abp.ExportManager;

public interface IExportService<TMetadata>
    where TMetadata : class
{
    Task<byte[]> ExportAsync<T>(IEnumerable<T> data,
        [CanBeNull] TMetadata metadata,
        CancellationToken cancellationToken = default);
}