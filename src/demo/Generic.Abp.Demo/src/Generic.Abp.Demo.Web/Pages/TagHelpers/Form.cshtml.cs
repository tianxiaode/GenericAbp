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
        [BindProperty] public BaseViewModel Base { get; set; }

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

        public class BaseViewModel
        {
            public string Name { get; set; }
            [DataType(DataType.Password)] public string Password { get; set; }
            public string PhoneNumber { get; set; }
            public int Age { get; set; }
            public bool Confirm { get; set; }
            [Display(Name = "BirthDay")] public DateTime BirthDateTime { get; set; }
            public CarType CarType { get; set; }

            [RadioGroup(2)] public CarType CarType2 { get; set; }
            [CheckboxGroup(2)] public List<CarType> CarType3 { get; set; }
        }

        public class IndexViewModel
        {
            [Required]
            [MaxLength(5)]
            [EmailAddress]
            [MinLength(2)]
            public string Email { get; set; }

            //[ReadOnlyInput] public string Password { get; set; }

            //[DisabledInput] public string Surname { get; set; }

            [Range(1, 100)] public int Age { get; set; } = 0;

            //[SelectItems(nameof(CountryList))]
            [Display(Name = "Country")] public List<string> Country { get; set; }

            //[SelectItems(nameof(CountryList))]
            //[MetroRadioGroup]
            public string RadioCountry { get; set; }

            [Required]
            [Display(Name = "My Car AccentColor")]
            public CarType MyCarType { get; set; }

            [Required]
            //[MetroRadioGroup]
            //[RadioOrCheckboxCols(3)]
            public CarType RadioCarType { get; set; }

            //[RadioOrCheckboxCols(3)] public List<CarType> CheckboxCarType { get; set; }


            [Display(Name = "Email")] public bool IsConfirm { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Day")]
            public DateTime Day { get; set; }

            public DateTime BirthDateTime { get; set; }


            //[TextArea]
            [StringLength(5)] [Required] public string Remarks { get; set; }

            public Address Address { get; set; }

            //[DynamicFormIgnore] public List<Address> AddressList { get; set; }
        }

        public class Address
        {
            public string Street { get; set; }
            public string City { get; set; }
            public string Postcode { get; set; }
            public string Country { get; set; }
        }
    }
}