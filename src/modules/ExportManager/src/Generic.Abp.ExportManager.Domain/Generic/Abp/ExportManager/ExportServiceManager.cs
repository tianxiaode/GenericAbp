using System;
using System.Collections.Generic;
using Generic.Abp.ExportManager.Csv;
using Generic.Abp.ExportManager.Excel;
using Generic.Abp.ExportManager.Pdf;

namespace Generic.Abp.ExportManager;

public class ExportServiceManager
{
    private readonly Dictionary<string, Func<IExportService>> _exportServices;

    public ExportServiceManager()
    {
        _exportServices = new Dictionary<string, Func<IExportService>>();
        // 注册默认的导出服务
        RegisterExportService("excel", () => new ExcelExportService());
        RegisterExportService("csv", () => new CsvExportService());
        RegisterExportService("pdf", () => new PdfExportService());
    }

    public void RegisterExportService(string fileType, Func<IExportService> factory)
    {
        _exportServices[fileType] = factory;
    }

    public IExportService GetExportService(string fileType)
    {
        if (_exportServices.ContainsKey(fileType))
        {
            return _exportServices[fileType]();
        }

        // 可以选择抛出异常或者返回默认的导出服务
        throw new ArgumentException($"Unsupported file type: {fileType}");
    }
}