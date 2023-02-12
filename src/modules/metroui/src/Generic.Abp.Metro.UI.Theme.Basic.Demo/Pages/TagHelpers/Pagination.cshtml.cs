using Generic.Abp.Metro.UI.TagHelpers.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Generic.Abp.Demo.Web.Pages.TagHelpers
{
    public class PaginationModel : PageModel
    {
        [BindProperty] public PagerModel PagerModel { get; set; }

        public Task OnGetAsync(int pageIndex, string sort)
        {
            PagerModel = new PagerModel(1000, 20, pageIndex, 20, "./TagHelpers/Pagination", sort);
            return Task.FromResult(Page());
        }
    }
}