using Generic.Abp.Extensions.Entities;
using Generic.Abp.FileManagement.Localization;
using Microsoft.Extensions.Localization;
using Volo.Abp.Threading;

namespace Generic.Abp.FileManagement.ExternalShares;

public class ExternalShareManager(
    IExternalShareRepository repository,
    IStringLocalizer<FileManagementResource> localizer,
    ICancellationTokenProvider cancellationTokenProvider)
    : EntityManagerBase<ExternalShare, IExternalShareRepository, FileManagementResource, ExternalShareSearchParams>(
        repository, localizer,
        cancellationTokenProvider)
{
}