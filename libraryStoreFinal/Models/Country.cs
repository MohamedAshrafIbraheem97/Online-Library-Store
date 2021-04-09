using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace libraryStoreFinal.Models
{
    public class Country
    {
        public int CountryID { get; set; }
        [Display(Name ="Country Name")]
        [Required]
        public string CountryName { get; set; }
    }
}