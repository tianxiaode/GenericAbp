using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Generic.Abp.Metro.UI.TagHelpers.Form;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Generic.Abp.Demo.Web.Pages
{
    public class IndexModel : DemoPageModel
    {
        [BindProperty]
        public IndexViewModel IndexView { get; set; }
        public List<SelectListItem> CountryList { get; set; } = new List<SelectListItem>
        {
            new() { Value = "CA", Text = "Canada"},
            new() { Value = "US", Text = "USA"},
            new() { Value = "UK", Text = "United Kingdom"},
            new() { Value = "RU", Text = "Russia"}
        };
        public enum Season
        {
            Spring,
            Summer,
            Autumn,
            Winter            
        }
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
            
            [SelectItems(nameof(CountryList))]
            [Display(Name = "Country")]
            public string Country { get; set; }
        
            [SelectItems(nameof(CountryList))]
            [Display(Name = "Neighbor Countries")]
            public List<string> NeighborCountries { get; set; }
            [Required]
            [Display(Name = "My Car Type")]
            public CarType MyCarType { get; set; }

            [Required]
            [Display(Name = "Your Car Type")]
            public CarType YourCarType { get; set; }
            public bool IsConfirm { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Day")]
            public DateTime Day { get; set; }

            [TextArea]
            [StringLength(5)]
            [Required]
            public string Remarks { get; set; }

        }

        public enum CarType
        {
            Sedan,
            Hatchback,
            StationWagon,
            Coupe
        }

    }
}