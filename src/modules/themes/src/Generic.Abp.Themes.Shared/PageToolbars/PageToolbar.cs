using JetBrains.Annotations;
using Volo.Abp;

namespace Generic.Abp.Themes.Shared.PageToolbars
{
    public class PageToolbar
    {
        public string PageName { get; }

        public PageToolbarContributorList Contributors { get; set; }

        public PageToolbar([NotNull] string pageName)
        {
            PageName = Check.NotNullOrEmpty(pageName, nameof(pageName));
            Contributors = new PageToolbarContributorList();
        }
    }
}
