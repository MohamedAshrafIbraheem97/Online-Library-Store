using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace libraryStoreFinal.Models
{
    public class Book
    {
        public int BookID { get; set; }
        [Display(Name ="Book Title")]
        [Required]
        public string BookTitle { get; set; }
        [Display(Name ="Deposite Number")]
        public string DepositeNumber { get; set; }

        [Display(Name ="Publish Year")]
        public DateTime? PublishYear { get; set; }
        [DefaultValue(0)]
        public int Quantity { get; set; }
        public int? PagesNumber { get; set; }
        public string KeyWords { get; set; }
        public string Notes { get; set; }

        [Display(Name ="Upload Book Cover")]
        public string BookCover { get; set; }
        [NotMapped]
        public HttpPostedFileBase bookCoverImage { get; set; }
        public string Code { get; set; }
        public string ISBN { get; set; }
        public string BarCode { get; set; }
        public decimal? Price { get; set; }
        public string PositionAtLibrary { get; set; }

        public Country Country { get; set; }
        public int CountryID { get; set; }

        public Form Form { get; set; }
        public int FormID { get; set; }

        public Language Language { get; set; }
        public int LanguageID { get; set; }


        public Publisher Publisher { get; set; }
        [Required]
        public int PublisherID { get; set; }

        public Status Status { get; set; }
        public int StatusID { get; set; }

        public virtual ICollection<BooksCategories> BooksCategories { get; set; }

        [NotMapped]
        public HttpPostedFileBase pdf { get; set; }
        //[NotMapped]
        //public string File { get; set; }
        //[NotMapped]
        //public long Size { get; set; }
        //[NotMapped]
        //public string Type { get; set; }
    }
}