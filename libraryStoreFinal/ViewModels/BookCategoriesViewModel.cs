using libraryStoreFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace libraryStoreFinal.ViewModels
{
    public class BookCategoriesViewModel
    {
        public Book Books { get; set; }
        public Category Categories { get; set; }
        public List<Category> CategoriesList { get; set; }

    }
}