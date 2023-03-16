﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.Metro.UI.Account.Web.Pages.Account;

public class AccessDeniedModel : AccountPageModel
{
    [BindProperty(SupportsGet = true)] public string ReturnUrl { get; set; }

    [BindProperty(SupportsGet = true)] public string ReturnUrlHash { get; set; }

    public virtual Task<IActionResult> OnGetAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }
}