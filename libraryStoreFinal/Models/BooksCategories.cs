using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace libraryStoreFinal.Models
{
    public class BooksCategories
    {
        [Key]
        [Column(Order =1)]
        public int BookId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int CategoryId { get; set; }


        public Book Books { get; set; }
        public Category Categories { get; set; }
    }
}