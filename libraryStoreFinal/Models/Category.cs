using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace libraryStoreFinal.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [NotMapped]
        public bool isActive { get; set; }


        public virtual ICollection<BooksCategories> BooksCategories { get; set; }

    }


    
}