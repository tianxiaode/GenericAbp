﻿using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity.Localization;

namespace Generic.Abp.Identity
{
    public abstract class IdentityController : AbpControllerBase
    {
        protected IdentityController()
        {
            LocalizationResource = typeof(IdentityResource);
        }
    }
}