using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.FileManagement.Resources.Dtos;

[Serializable]
public class FolderCreateDto : FolderCreateOrUpdateDto
{
    [Required]
    [DisplayName("Folder:Parent")]
    public Guid ParentId { get; set; } = default!;
}