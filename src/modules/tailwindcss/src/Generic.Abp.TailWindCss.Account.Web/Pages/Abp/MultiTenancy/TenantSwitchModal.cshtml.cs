using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Generic.Abp.TailWindCss.Account.Web.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.TailWindCss.Account.Web.Pages.Abp.MultiTenancy;

public class TenantSwitchModalModel : AbpPageModel
{
    [BindProperty] public TenantInfoModel Input { get; set; }

    protected ITenantStore TenantStore { get; }
    protected AbpAspNetCoreMultiTenancyOptions Options { get; }

    public TenantSwitchModalModel(
        ITenantStore tenantStore,
        IOptions<AbpAspNetCoreMultiTenancyOptions> options)
    {
        TenantStore = tenantStore;
        Options = options.Value;
        LocalizationResourceType = typeof(TailWindCssAccountWebResource);
    }

    public virtual async Task OnGetAsync()
    {
        Input = new TenantInfoModel();

        if (CurrentTenant.IsAvailable)
        {
            var tenant = await TenantStore.FindAsync(CurrentTenant.GetId());
            Input.Name = tenant?.Name;
        }
    }

    public virtual async Task<NoContentResult> OnPostAsync()
    {
        Guid? tenantId = null;
        if (!Input.Name.IsNullOrEmpty())
        {
            var tenant = await TenantStore.FindAsync(Input.Name);
            if (tenant == null)
            {
                throw new UserFriendlyException(L["GivenTenantIsNotExist", Input.Name]);
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L["GivenTenantIsNotAvailable", Input.Name]);
            }

            tenantId = tenant.Id;
        }

        AbpMultiTenancyCookieHelper.SetTenantCookie(HttpContext, tenantId, Options.TenantKey);
        return NoContent();
    }

    public class TenantInfoModel
    {
        [Required] public string Name { get; set; }
    }
}