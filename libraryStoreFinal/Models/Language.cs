using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace libraryStoreFinal.Models
{
    public class Language
    {
        public int LanguageID { get; set; }
        [Display(Name ="Langauage Name")]
        [Required]
        public string LanguageName { get; set; }
    }
}