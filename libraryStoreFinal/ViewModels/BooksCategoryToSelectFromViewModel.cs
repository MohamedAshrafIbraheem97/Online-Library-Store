using libraryStoreFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace libraryStoreFinal.ViewModels
{
    public class BooksCategoryToSelectFromViewModel
    {
        public Book Book { get; set; }
        public SelectedCategory SelectedCategory { get; set; }
        public SelectedCountry SelectedCountry { get; set; }
    }
}