﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generic.Abp.Account.Web.Pages.Account
{
    public class SendSecurityCodeModel : AccountPageModel
    {
        public List<SelectListItem> Providers { get; set; }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return RedirectToPage("./Login");
            }

            return Page();

            //CheckCurrentTenant(await SignInManager.GetVerifiedTenantIdAsync());

            //Providers = (await UserManager.GetValidTwoFactorProvidersAsync(user))
            //    .Select(userProvider =>
            //        new SelectListItem
            //        {
            //            Text = userProvider,
            //            Value = userProvider
            //        }).ToList();

            //return View(
            //    new SendSecurityCodeViewModel
            //    {
            //        ReturnUrl = returnUrl,
            //        RememberMe = rememberMe
            //    }
            //);
        }

        public virtual Task<IActionResult> OnPostAsync()
        {
            return Task.FromResult<IActionResult>(Page());
        }
    }
}
