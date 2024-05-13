using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;

namespace Generic.Abp.Tailwind.Areas.Account.Controllers.Models;

public class UserLoginInfo
{
    [Required] [StringLength(255)] public string UserNameOrEmailAddress { get; set; } = string.Empty;

    [Required]
    [StringLength(32)]
    [DataType(DataType.Password)]
    [DisableAuditing]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}