using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.Demo.Web.Pages
{
    public class IndexModel : DemoPageModel
    {
        [BindProperty] public IndexViewModel IndexView { get; set; }

        public List<SelectListItem> CountryList { get; set; } = new List<SelectListItem>
        {
            new() { Value = "CA", Text = "Canada" },
            new() { Value = "US", Text = "USA" },
            new() { Value = "UK", Text = "United Kingdom" },
            new() { Value = "RU", Text = "Russia" }
        };

        public enum CarType
        {
            Sedan,
            Hatchback,
            StationWagon,
            Coupe
        }

        public void OnGet()
        {
            IndexView = new IndexViewModel
            {
                Age = 10,
                Country = new List<string>() { CountryList[1].Value },
                Day = DateTime.Now,
                Email = "abc",
                IsConfirm = true,
                //MyCarType = CarType.Hatchback,
                //Password = "dsdf",
                //RadioCarType = CarType.Coupe,
                //RadioCountry = CountryList[2].Value,
                //CheckboxCarType = new List<CarType> { CarType.Coupe, CarType.StationWagon },
                Remarks = "sdfsfsdfssdfsdf"
            };
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
            //[MetroRadioOrCheckboxCols(3)]
            public CarType RadioCarType { get; set; }

            //[MetroRadioOrCheckboxCols(3)] public List<CarType> CheckboxCarType { get; set; }


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