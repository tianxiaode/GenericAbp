using Volo.Abp.Application.Services;
using Volo.Abp.AuditLogging.Localization;

namespace Generic.Abp.AuditLogging;

public class AuditLoggingAppService : ApplicationService
{
    public AuditLoggingAppService()
    {
        ObjectMapperContext = typeof(GenericAbpAuditLoggingApplicationModule);
        LocalizationResource = typeof(AuditLoggingResource);
    }
}