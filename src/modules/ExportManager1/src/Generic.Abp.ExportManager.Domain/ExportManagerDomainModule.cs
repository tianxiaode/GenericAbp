using Generic.Abp.ExportManager.Settings;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;

namespace Generic.Abp.ExportManager;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(ExportManagerDomainSharedModule)
)]
public class ExportManagerDomainModule : AbpModule
{
    public void ConfigureServices(IServiceCollection services)
    {
        // 注册导出服务管理器
        services.AddSingleton<ExportServiceManager>();

        // 注册默认的导出服务


        services.AddSingleton<IExportService, ExcelExportService>();
        services.AddSingleton<IExportService, PdfExportService>();
        services.AddSingleton<IExportService, CsvExportService>();
    }
}