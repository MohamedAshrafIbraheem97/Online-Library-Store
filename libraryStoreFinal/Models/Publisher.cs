using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace libraryStoreFinal.Models
{
    public class Publisher
    {
        public int PublisherID { get; set; }
        [Display(Name ="Publisher Name")]
        [Required]
        public string PublisherName { get; set; }
    }
}