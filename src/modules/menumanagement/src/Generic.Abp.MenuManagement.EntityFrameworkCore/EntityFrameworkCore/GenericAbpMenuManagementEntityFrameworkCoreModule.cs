﻿using Generic.Abp.Extensions.EntityFrameworkCore;
using Generic.Abp.MenuManagement.EntityFrameworkCore.Menus;
using Generic.Abp.MenuManagement.Menus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.Modularity;

namespace Generic.Abp.MenuManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(GenericAbpMenuManagementDomainModule),
        typeof(GenericAbpExtensionsEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class GenericAbpMenuManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<MenuManagementDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddRepository<Menu, MenuRepository>();
            });
        }
    }
}