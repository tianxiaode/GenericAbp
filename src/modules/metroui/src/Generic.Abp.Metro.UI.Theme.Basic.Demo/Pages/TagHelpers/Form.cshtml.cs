using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;

namespace Generic.Abp.Demo.Web.Pages.TagHelpers
{
    public class FormModel : PageModel
    {
        [BindProperty] public FormViewModel Form { get; set; }

        public void OnGet()
        {
        }

        public enum CarType
        {
            Sedan,
            Hatchback,
            StationWagon,
            Coupe
        }

        public List<SelectListItem> CountryList { get; set; } = new List<SelectListItem>
        {
            new() { Value = "CA", Text = "Canada" },
            new() { Value = "US", Text = "USA" },
            new() { Value = "UK", Text = "United Kingdom" },
            new() { Value = "RU", Text = "Russia" }
        };

        public List<SelectListItem> CityList { get; set; } = new List<SelectListItem>
        {
            new() { Value = "NY", Text = "New York" },
            new() { Value = "LDN", Text = "London" },
            new() { Value = "IST", Text = "Istanbul" },
            new() { Value = "MOS", Text = "Moscow" }
        };

        public class FormViewModel
        {
            [HiddenInput] public Guid Id { get; set; }

            [Required]
            [StringLength(128, MinimumLength = 4)]
            public string Name { get; set; }

            [FormContentGroup("base")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public string PhoneNumber { get; set; }

            [Required] public int Age { get; set; }
            [DisplayOrder(20000)] public bool Confirm { get; set; }

            [Display(Name = "BirthDay")]
            [DataType(DataType.Date)]
            public DateTime BirthDateTime { get; set; }

            public DateTime UpdateDateTime { get; set; }
            public CarType CarType { get; set; }
            [RadioGroup(4)] public CarType CarType2 { get; set; }
            [CheckboxGroup(2)] public List<CarType> CarType3 { get; set; }
            [SelectItems(nameof(CountryList))] public string Country { get; set; }

            [SelectItems(nameof(CityList))] public List<string> City { get; set; }

            [SelectItems(nameof(CityList))]
            [CheckboxGroup(4)]
            public List<string> City1 { get; set; }

            [TagInput(5)] public List<string> Tags { get; set; }
            public string[] Tags2 { get; set; }

            [TextArea] public string Remarks { get; set; }

            public Address Address { get; set; }
        }

        public class Address
        {
            [FormContentGroup("address")] public string Street { get; set; }
            [FormContentGroup("address")] public string City { get; set; }
            [FormContentGroup("address")] public string Postcode { get; set; }
            [FormContentGroup("address")] public string Country { get; set; }
        }
    }
}