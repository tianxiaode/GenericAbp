using System;
using System.Collections.Generic;

namespace QuickTemplate.Controllers;

public class ExportInputDto : IExportInputDto
{
    public string Format { get; set; }
    public bool IsAll { get; set; }
    public bool IsSelected { get; set; }
    public bool IsSearch { get; set; }
    public bool IsAsync { get; set; }
    public List<Guid> Ids { get; set; }
}