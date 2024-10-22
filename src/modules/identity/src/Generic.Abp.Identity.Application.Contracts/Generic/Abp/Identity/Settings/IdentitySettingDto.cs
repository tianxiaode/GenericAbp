using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Generic.Abp.Identity.Settings;

[Serializable]
public class IdentitySettingDto
{
    //密码设置
    [Required]
    [DisplayName("DisplayName:Abp.Identity.Password.RequiredLength")]
    [Range(6, 128)]

    public int RequiredLength { get; set; }

    [Required]
    [DisplayName("DisplayName:Abp.Identity.Password.RequiredUniqueChars")]
    [Range(0, int.MaxValue)]
    public int RequiredUniqueChars { get; set; }

    [Required]
    [DisplayName("DisplayName:Abp.Identity.Password.RequireNonAlphanumeric")]
    public bool RequireNonAlphanumeric { get; set; }

    [Required]
    [DisplayName("DisplayName:Abp.Identity.Password.RequireLowercase")]
    public bool RequireLowercase { get; set; }

    [Required]
    [DisplayName("DisplayName:Abp.Identity.Password.RequireUppercase")]
    public bool RequireUppercase { get; set; }

    [Required]
    [DisplayName("DisplayName:Abp.Identity.Password.RequireDigit")]
    public bool RequireDigit { get; set; }

    //密码更新设置
    [Required]
    [DisplayName("DisplayName:Abp.Identity.Password.ForceUsersToPeriodicallyChangePassword")]
    public bool ForceUsersToPeriodicallyChangePassword { get; set; }

    [Required]
    [DisplayName("DisplayName:Abp.Identity.Password.PasswordChangePeriodDays")]
    [Range(0, int.MaxValue)]
    public int PasswordChangePeriodDays { get; set; }


    //锁定设置
    [Required]
    [DisplayName("DisplayName:Abp.Identity.Lockout.AllowedForNewUsers")]
    public bool AllowedForNewUsers { get; set; }


    [Required]
    [DisplayName("DisplayName:Abp.Identity.Lockout.LockoutDuration")]
    [Range(0, int.MaxValue)]
    public int LockoutDuration { get; set; }

    [Required]
    [DisplayName("DisplayName:Abp.Identity.Lockout.MaxFailedAccessAttempts")]
    [Range(0, int.MaxValue)]
    public int MaxFailedAccessAttempts { get; set; }

    //登录设置
    [Required]
    [DisplayName("DisplayName:Abp.Identity.SignIn.RequireConfirmedEmail")]
    public bool RequireConfirmedEmail { get; set; }

    [Required]
    [DisplayName("DisplayName:Abp.Identity.SignIn.EnablePhoneNumberConfirmation")]
    public bool EnablePhoneNumberConfirmation { get; set; }

    [Required]
    [DisplayName("DisplayName:Abp.Identity.SignIn.RequireConfirmedPhoneNumber")]
    public bool RequireConfirmedPhoneNumber { get; set; }


    //用户设置

    [Required]
    [DisplayName("DisplayName:Abp.Identity.User.IsEmailUpdateEnabled")]
    public bool IsEmailUpdateEnabled { get; set; }


    [Required]
    [DisplayName("DisplayName:Abp.Identity.User.IsUserNameUpdateEnabled")]
    public bool IsUserNameUpdateEnabled { get; set; }
}