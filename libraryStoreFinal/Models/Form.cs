using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace libraryStoreFinal.Models
{
    public class Form
    {
        public int FormID { get; set; }
        [Display( Name ="Form Title")]
        [Required]
        public string FormTitle { get; set; }
    }
}