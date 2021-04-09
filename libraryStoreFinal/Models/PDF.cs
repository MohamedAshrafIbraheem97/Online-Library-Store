using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace libraryStoreFinal.Models
{
    public class PDF
    {
        public Book BookId { get; set; }
        public HttpPostedFileBase files { get; set; }

        public string File { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }
    }
}