using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace libraryStoreFinal.Models
{
    public class Status
    {
        public int StatusID { get; set; }
        [Display(Name ="Status Name")]
        [Required]
        public string StatusName { get; set; }
    }
}