using System.ComponentModel.DataAnnotations;
using Generic.Abp.Metro.UI.TagHelpers.Form;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.Demo.Web.Pages
{
    public class IndexModel : DemoPageModel
    {
        [BindProperty]
        public IndexViewModel IndexView { get; set; }
        public void OnGet()
        {
            
        }

        public class IndexViewModel
        {
            [Required]
            [MaxLength(5)]
            [EmailAddress]
            [MinLength(2)]
            public string Email { get; set; }

            [ReadOnlyInput]
            public string Password { get; set; }

            [DisabledInput]
            public string Surname { get; set; }

            [Range(1, 100)] public int Age { get; set; } = 0;
            public string Phone1 { get; set; }
            
            public bool Name { get; set; }
            [TextArea]
            [StringLength(5)]
            [Required]
            public string Demo { get; set; }
            
        }

    }
}