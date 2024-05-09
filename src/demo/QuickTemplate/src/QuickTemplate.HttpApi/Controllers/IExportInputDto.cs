using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace QuickTemplate.Controllers;

public interface IExportInputDto
{
    /**
     * Export format
     */
    [Required]
    string Format { get; set; }

    /**
     * Export all data
     */
    bool IsAll { get; set; }

    /**
     * Export selected data
     */
    bool IsSelected { get; set; }

    /**
     * Export  Search data
     */
    bool IsSearch { get; set; }

    /**
     * Export data asynchronously
     */
    bool IsAsync { get; set; }

    [CanBeNull] List<Guid> Ids { get; set; }
}