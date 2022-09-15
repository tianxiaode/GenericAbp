using System;

namespace Generic.Abp.Identity.Users;

[Serializable]
public class PasswordPolicyDto
{
    public int RequiredLength { get; set; }
    //public int RequiredUniqueChars { get; set; }
    public bool RequireNonAlphanumeric { get; set; }
    public bool RequireLowercase { get; set; }
    public bool RequireUppercase { get; set; }
    public bool RequireDigit { get; set; }
}