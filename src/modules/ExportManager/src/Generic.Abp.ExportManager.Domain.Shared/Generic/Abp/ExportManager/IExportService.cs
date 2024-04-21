using Generic.Abp.ExportManager.Metadata;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Abp.ExportManager;

public interface IExportService<TMetadata>
    where TMetadata : class
{
    Task<byte[]> ExportAsync<T>(IEnumerable<T> data,
        [CanBeNull] TMetadata schema,
        CancellationToken cancellationToken = default);
}