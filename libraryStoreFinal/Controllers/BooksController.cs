using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using libraryStoreFinal.Models;
using PagedList;
using libraryStoreFinal.ViewModels;
using System.IO;

namespace libraryStoreFinal.Controllers
{
    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Books
        //public ActionResult Index()
        //{
        //    var books = db.Books.Include(b => b.Country).Include(b => b.Form).Include(b => b.Language).Include(b => b.Publisher).Include(b => b.Status);
        //    return View(books.ToList());
        //}

        public ActionResult Index(string sortOrder,
                                    string currentFilter,
                                    string searchString,
                                    int? page)
        {
            var searchForBooksWithTitle = db.Books.Include(b => b.Country).Include(b => b.Form).Include(b => b.Language).Include(b => b.Publisher).Include(b => b.Status);

            
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;
            //if (searchString == null)
            //    searchString = currentFilter;


            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                searchForBooksWithTitle = db.Books.Where(b => b.BookTitle.Contains(searchString)||
                                                                b.Code.Contains(searchString) ||
                                                                b.ISBN.Contains(searchString) ||
                                                                b.KeyWords.Contains(searchString) ||
                                                                b.Notes.Contains(searchString)).Include(b => b.Country).Include(b => b.Form).Include(b => b.Language).Include(b => b.Publisher).Include(b => b.Status);

                
            }  


            switch (sortOrder)
            {
                case "title_desc":
                    searchForBooksWithTitle = searchForBooksWithTitle.OrderByDescending(b => b.BookTitle);
                    break;
                case "Date":
                    searchForBooksWithTitle = searchForBooksWithTitle.OrderBy(b => b.PublishYear);

                    break;
                case "date_desc":
                    searchForBooksWithTitle = searchForBooksWithTitle.OrderByDescending(b => b.PublishYear);
                    break;
                default:
                    searchForBooksWithTitle = searchForBooksWithTitle.OrderBy(b => b.BookTitle);
                    break;
            }

            int pageSize = 10;
            int currentPage = (page ?? 1);
            var x = searchForBooksWithTitle.ToPagedList(currentPage, pageSize);

            return View(x);

        }

        
        
        public ActionResult Borrow(int id)
        {
            var availbleToRent = db.Books.SingleOrDefault(b => b.BookID == id);
            if (availbleToRent.StatusID == 2)
            {

                TempData["AvailableToBorrow"] = "Book Is Not Available";
                return RedirectToAction("Index");
                //return Json(new { result }/*, JsonRequestBehavior.AllowGet*/);

            }
            else if (availbleToRent.StatusID == 1)
            {
                availbleToRent.Quantity -= 1;
                if (availbleToRent.Quantity == 0)
                {
                    availbleToRent.StatusID = 2;
                }
                db.SaveChanges();
                //ViewBag.borrowed = true;
                return RedirectToAction("Index");

                //return Json(new { result }/*, JsonRequestBehavior.AllowGet*/);

            }
            else
            {
                TempData["AvailableToBorrow"] = "Book Is Not Available";
                return RedirectToAction("Index");
            }
            //return Json(new { result }/*, JsonRequestBehavior.AllowGet*/);


        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.FirstOrDefault(b => b.BookID == id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            //ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName");
            ViewBag.FormID = new SelectList(db.Forms, "FormID", "FormTitle");
            ViewBag.LanguageID = new SelectList(db.Languages, "LanguageID", "LanguageName");
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "PublisherName");
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName");

            BooksCategoryToSelectFromViewModel BooksCategory = new BooksCategoryToSelectFromViewModel
            {
                Book = null,
                SelectedCategory = new SelectedCategory { CategoriesListToSelectFrom = db.Categories.ToList() },
                SelectedCountry = new SelectedCountry { CountriesList = db.Countries.ToList()},
                
            };

            //SelectedCategory sg = new SelectedCategory { Categories = db.Categories.ToList() };
            return View(BooksCategory);
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BooksCategoryToSelectFromViewModel bookAndCategpries/*,int [] categoriesIds*/)
        {
            if (ModelState.IsValid)
            {
                if (bookAndCategpries.Book.Quantity > 0)
                    bookAndCategpries.Book.StatusID = 1;
                else
                    bookAndCategpries.Book.StatusID = 2;
                if (bookAndCategpries.Book.bookCoverImage != null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(bookAndCategpries.Book.bookCoverImage.FileName);
                    var fileExtention = Path.GetExtension(bookAndCategpries.Book.bookCoverImage.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssff") + fileExtention;

                    bookAndCategpries.Book.BookCover = "~/Images/bookCovers/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/bookCovers/"), fileName);
                    bookAndCategpries.Book.bookCoverImage.SaveAs(fileName);
                    
                }
                
                if (bookAndCategpries.SelectedCategory.SelectedCategories.Count() != 0)
                {
                    foreach (var item in bookAndCategpries.SelectedCategory.SelectedCategories)
                    {
                        BooksCategories bookCategory = new BooksCategories()
                        {
                            BookId = bookAndCategpries.Book.BookID,
                            CategoryId = item
                        };
                        db.BooksCategories.Add(bookCategory);
                        db.SaveChanges();

                    }
                }
                //if (bookAndCategpries.SelectedCountry.SelectedCountries != 0)
                //{
                //    bookAndCategpries.Book.CountryID = bookAndCategpries.SelectedCountry.SelectedCountries;
                //}
                bookAndCategpries.Book.CountryID = bookAndCategpries.SelectedCountry.SelectedCountries[0];

                db.Books.Add(bookAndCategpries.Book);
                db.SaveChanges();

                //foreach (var item in categoriesIds)
                //{
                //    BooksCategories bookCategory = new BooksCategories()
                //    {
                //        BookId = book.BookID,
                //        CategoryId = item
                //    };
                //    db.BooksCategories.Add(bookCategory);
                //}


                return RedirectToAction("Index");
            }

            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", bookAndCategpries.Book.CountryID);
            ViewBag.FormID = new SelectList(db.Forms, "FormID", "FormTitle", bookAndCategpries.Book.FormID);
            ViewBag.LanguageID = new SelectList(db.Languages, "LanguageID", "LanguageName", bookAndCategpries.Book.LanguageID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "PublisherName", bookAndCategpries.Book.PublisherID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", bookAndCategpries.Book.StatusID);
            return View(bookAndCategpries);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", book.CountryID);
            ViewBag.FormID = new SelectList(db.Forms, "FormID", "FormTitle", book.FormID);
            ViewBag.LanguageID = new SelectList(db.Languages, "LanguageID", "LanguageName", book.LanguageID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "PublisherName", book.PublisherID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", book.StatusID);
            ViewBag.Categories = db.Categories.ToList();
            //ViewBag.selectedCategories = db.BooksCategories.Where(bc => bc.BookId == id).ToList();
            //ViewBag.Categories = db.Categories.Select(bc => new Category {
            //    CategoryID = bc.CategoryID,
            //    CategoryName = bc.CategoryName,
            //    isActive = bc.BooksCategories.Any(b => b.BookId == id) ? true : false

            //}).ToList();


            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book book, int[] categoriesIds)
        {
            if (ModelState.IsValid)
            {
                if (book.Quantity > 0)
                    book.StatusID = 1;
                else
                    book.StatusID = 2;
                //foreach (var item in categoriesIds)
                //{
                //    BooksCategories bookCategory = new BooksCategories()
                //    {
                //        BookId = book.BookID,
                //        CategoryId = item
                //    };
                //    db.BooksCategories.Add(bookCategory);
                //}
                if (book.bookCoverImage != null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(book.bookCoverImage.FileName);
                    var fileExtention = Path.GetExtension(book.bookCoverImage.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssff") + fileExtention;

                    book.BookCover = "~/Images/bookCovers/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/bookCovers/"), fileName);
                    book.bookCoverImage.SaveAs(fileName);
                }

                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", book.CountryID);
            ViewBag.FormID = new SelectList(db.Forms, "FormID", "FormTitle", book.FormID);
            ViewBag.LanguageID = new SelectList(db.Languages, "LanguageID", "LanguageName", book.LanguageID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "PublisherName", book.PublisherID);
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "StatusName", book.StatusID);
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //public ActionResult Upload(HttpPostedFileBase uploadedPdf)
        //{
        //    if(uploadedPdf.ContentLength > 0)
        //    {
        //        string pdfFileName = Path.GetFileName(uploadedPdf.FileName);
        //        string folderPath = Path.Combine(Server.MapPath("~/PDFs"), pdfFileName);

        //    }
        //    return RedirectToAction("Index");
        //}
        
        //[HttpGet]
        //public ActionResult Upload()
        //{
        //    List<Book> ObjFiles = new List<Book>();
        //    foreach (string strfile in Directory.GetFiles(Server.MapPath("~/PDFs")))
        //    {
        //        FileInfo fi = new FileInfo(strfile);
        //        Book obj = new Book();
        //        obj.File = fi.Name;
        //        obj.Size = fi.Length;
        //        obj.Type = GetFileTypeByExtension(fi.Extension);
        //        ObjFiles.Add(obj);
        //    }

        //    return View(ObjFiles);
        //}
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase book)
        {
            if ( book.ContentLength > 0 || TempData.ContainsKey("bookId") != false)
            {
                string [] allowedExtention = { ".pdf", ".png", ".jpg", ".docx" };

                var bookId = TempData["bookId"].ToString();
                var fileName = Path.GetFileName(book.FileName);
                var fileExtention = Path.GetExtension(book.FileName).ToLower();

                //to check if uploaded file extention is in my extentions or not
                int counterForExtentions = 0;
                for (int i = 0; i < allowedExtention.Length; i++)
                {
                    if (fileExtention != allowedExtention[i])
                        counterForExtentions += 1;
                    else
                        break;
                    
                }

                if (counterForExtentions == allowedExtention.Count())
                {
                    TempData["exetentionError"] = "Please Upload PDF ,png ,jpg or Docx File";
                    return RedirectToAction("Edit", new { id = bookId });
                }
                
                var newName =  bookId + fileExtention;
                string path = Path.Combine(Server.MapPath("~/PDFs"),newName);
                book.SaveAs(path);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Edit", new { id = TempData["bookId"] });
        }

        public ActionResult Download(int id)
        {
            //var bookId = TempData["bookId"].ToString();
            try {
                var fileName = id + ".pdf";
                string fullPath = Path.Combine(Server.MapPath("~/PDFs"), fileName);
                byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch(FileNotFoundException es)
            {
                TempData["PdfNotFound"] = "Book is Not Uploaded";
                return RedirectToAction("Index");

            }
        }

        //public List<string> GetFiles()
        //{
        //    var dir = new System.IO.DirectoryInfo(Server.MapPath("~/PDFs"));
        //    System.IO.FileInfo[] fileNames = dir.GetFiles("*.pdf");

        //    return View();
        //}
        //private string GetFileTypeByExtension(string fileExtension)
        //{
        //    switch (fileExtension.ToLower())
        //    {
        //        case ".docx":
        //        case ".doc":
        //            return "Microsoft Word Document";
        //        case ".xlsx":
        //        case ".xls":
        //            return "Microsoft Excel Document";
        //        case ".txt":
        //            return "Text Document";
        //        case ".jpg":
        //        case ".png":
        //            return "Image";
        //        default:
        //            return "Unknown";
        //    }
        //}



        //public FileResult Download(string fileName)
        //{
        //    string fullPath = Path.Combine(Server.MapPath("~/PDFs"), fileName);
        //    byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        //}
        //private string GetFileTypeByExtension(string fileExtension)
        //{
        //    switch (fileExtension.ToLower())
        //    {
        //        case ".docx":
        //        case ".doc":
        //            return "Microsoft Word Document";
        //        case ".xlsx":
        //        case ".xls":
        //            return "Microsoft Excel Document";
        //        case ".txt":
        //            return "Text Document";
        //        case ".jpg":
        //        case ".png":
        //            return "Image";
        //        default:
        //            return "Unknown";
        //    }
        //}
    }
}
