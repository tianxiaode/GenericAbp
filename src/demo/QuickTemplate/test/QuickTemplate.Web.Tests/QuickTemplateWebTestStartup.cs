using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace QuickTemplate;

public class QuickTemplateWebTestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<QuickTemplateWebTestModule>();
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.InitializeApplication();
    }
}
