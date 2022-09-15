using System;

namespace Generic.Abp.Identity.Users;

[Serializable]
public class LookupPolicyDto
{
    public bool AllowedForNewUsers { get; set; }
    public int LockoutDuration { get; set; }
    public int MaxFailedAccessAttempts { get; set; }

}