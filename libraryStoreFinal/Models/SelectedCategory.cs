using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace libraryStoreFinal.Models
{
    public class SelectedCategory
    {
        [Display(Name = "Select Categories")]
        public List<Category> CategoriesListToSelectFrom { get; set; }
        public int[] SelectedCategories { get; set; }
    }

    public class SelectedCountry
    {
        [Display(Name ="Select Country")]
        public List<Country> CountriesList { get; set; }
        public int[] SelectedCountries { get; set; }
    }
}