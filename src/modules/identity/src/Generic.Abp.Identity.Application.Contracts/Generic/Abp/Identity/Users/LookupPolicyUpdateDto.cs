using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.Identity.Users;

public class LookupPolicyUpdateDto
{
    [Required]
    [DisplayName("DisplayName:Abp.Identity.Lockout.AllowedForNewUsers")]
    public bool AllowedForNewUsers { get; set; }

    [Required]
    [DisplayName("DisplayName:Abp.Identity.Lockout.LockoutDuration")]
    [Range(1800, 72000)]
    public int LockoutDuration { get; set; }
    
    [Required]
    [DisplayName("DisplayName:Abp.Identity.Lockout.MaxFailedAccessAttempts")]
    [Range(3,10)]
    public int MaxFailedAccessAttempts { get; set; }

}