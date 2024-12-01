using Generic.Abp.Extensions.Entities;
using Volo.Abp.Threading;

namespace Generic.Abp.FileManagement.ExternalShares;

public class ExternalShareManager(
    IExternalShareRepository repository,
    ICancellationTokenProvider cancellationTokenProvider)
    : EntityManagerBase<ExternalShare, IExternalShareRepository>(repository, cancellationTokenProvider)
{
}