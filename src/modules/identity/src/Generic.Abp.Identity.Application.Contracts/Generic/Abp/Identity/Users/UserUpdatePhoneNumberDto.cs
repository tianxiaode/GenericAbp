﻿using System.ComponentModel;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Generic.Abp.Identity.Users;

public class UserUpdatePhoneNumberDto
{
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
    [DisplayName("DisplayName:PhoneNumber")]
    public string Value { get; set; }

}