﻿using Generic.Abp.Extensions;
using Volo.Abp.AuditLogging;
using Volo.Abp.Modularity;

namespace Generic.Abp.AuditLogging
{
    [DependsOn(
        typeof(AbpAuditLoggingDomainModule),
        typeof(GenericAbpExtensionsDomainModule)
    )]
    public class GenericAbpAuditLoggingDomainModule : AbpModule
    {
    }
}