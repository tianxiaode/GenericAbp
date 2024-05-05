using System;
using System.Collections.Generic;
using System.Text;
using Generic.Abp.Host.Localization;
using Volo.Abp.Application.Services;

namespace Generic.Abp.Host;

/* Inherit your application services from this class.
 */
public abstract class HostAppService : ApplicationService
{
    protected HostAppService()
    {
        LocalizationResource = typeof(HostResource);
    }
}
