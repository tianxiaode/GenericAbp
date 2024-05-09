using System.IO;

namespace QuickTemplate.Controllers;

public interface IExportDto
{
    bool IsAsync { get; }
    bool Success { get; }
    string File { get; }
}