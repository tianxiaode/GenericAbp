using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.ExternalAuthentication.dtos;

[Serializable]
public class ExternalSettingUpdateDto
{
    [Required]
    //[Display(Name = "NewUserPrefix")]
    public string NewUserPrefix { get; set; } = string.Empty;

    [Required]
    //[DisplayName("NewUserEmailSuffix")]
    public string NewUserEmailSuffix { get; set; } = string.Empty;

    public List<ExternalProviderDto> Providers { get; set; } = [];
}