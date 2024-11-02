using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AuditLogging.Localization;

namespace Generic.Abp.AuditLogging;

public class AuditLoggingController : AbpControllerBase
{
    public AuditLoggingController()
    {
        LocalizationResource = typeof(AuditLoggingResource);
    }
}