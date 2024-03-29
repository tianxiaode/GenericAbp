﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Identity;

namespace Generic.Abp.Identity
{
    [DependsOn(
        typeof(AbpIdentityApplicationModule),
        typeof(GenericAbpIdentityDomainModule),
        typeof(GenericAbpIdentityApplicationContractsModule),
        typeof(AbpAutoMapperModule)
    )]
    public class GenericAbpIdentityApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<GenericAbpIdentityApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<IdentityApplicationAutoMapperProfile>(validate: true);
            });
        }
    }
}