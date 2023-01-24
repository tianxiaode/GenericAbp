using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.Metro.UI.Theme.Basic.Themes.Basic.Components.MetroMeta;

public class MetroMetaViewComponent: AbpViewComponent
{
    public virtual  Task<IViewComponentResult> InvokeAsync()
    {
        return Task.FromResult(View("~/Themes/Basic/Components/MetroMeta/Default.cshtml") as IViewComponentResult);
    }

}