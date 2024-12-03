using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.FileManagement.ExternalShares.Dtos;

[Serializable]
public class ExternalShareGetTokenDto
{
    [Required]
    [DisplayName("ExternalShare:Password")]
    public string Password { get; set; } = default!;
}