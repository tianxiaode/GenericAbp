namespace QuickTemplate.Controllers;

public class ExportDto : IExportDto
{
    public string File { get; set; }

    public bool Success { get; set; } = true;

    public bool IsAsync { get; set; } = false;
}