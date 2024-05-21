using Generic.Abp.Domain;
using Generic.Abp.ExportManager.Csv;
using Generic.Abp.ExportManager.Excel;
using Generic.Abp.ExportManager.Pdf;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Generic.Abp.ExportManager
{
    [DependsOn(
        typeof(GenericAbpDddDomainModule),
        typeof(GenericAbpExportManagerDomainSharedModule)
    )]
    public class GenericAbpExportManagerDomainModule : AbpModule
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
}