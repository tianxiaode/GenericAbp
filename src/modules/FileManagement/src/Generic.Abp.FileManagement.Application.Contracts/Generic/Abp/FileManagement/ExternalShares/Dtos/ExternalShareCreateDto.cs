using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.FileManagement.ExternalShares.Dtos;

[Serializable]
public class ExternalShareCreateDto
{
    [Required] [DisplayName("Resource")] public Guid ResourceId { get; set; }
}