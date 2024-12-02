using Generic.Abp.Extensions.Entities;

namespace Generic.Abp.FileManagement.ExternalShares;

public interface IExternalShareRepository : IExtensionRepository<ExternalShare, ExternalShareSearchParams>
{
}