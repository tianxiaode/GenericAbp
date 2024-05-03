using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generic.Abp.TailWindCss.Account.Web.Providers;

public interface ILayoutMenuProvider
{
    Task<List<LayoutMenu>> GetMainMenusAsync();
    Task<List<LayoutMenu>> GetMobileMenusAsync();
}