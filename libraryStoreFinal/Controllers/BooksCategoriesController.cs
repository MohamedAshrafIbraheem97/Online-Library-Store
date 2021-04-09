using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using libraryStoreFinal.Models;
using libraryStoreFinal.ViewModels;

namespace libraryStoreFinal.Controllers
{
    public class BooksCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BooksCategories
        public ActionResult Index()
        {
            var booksCategories = db.BooksCategories.Include(b => b.Books).Include(b => b.Categories);
            return View(booksCategories.ToList());
        }

        // GET: BooksCategories/Details/5
        public ActionResult Details(int? bookId, int? categoryId)
        {
            if (bookId == null && categoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BooksCategories booksCategories = db.BooksCategories.Find(bookId, categoryId);
            if (booksCategories == null)
            {
                return HttpNotFound();
            }
            BookCategoriesViewModel vm = new BookCategoriesViewModel()
            {
                Books = db.Books.SingleOrDefault(b => b.BookID == booksCategories.BookId),
                Categories = db.Categories.SingleOrDefault(c => c.CategoryID == booksCategories.CategoryId),
                CategoriesList = null 
            };
            return View(vm);
        }

        // GET: BooksCategories/Create
        public ActionResult Create()
        {
            ViewBag.BookId = new SelectList(db.Books, "BookID", "BookTitle");
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: BooksCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookId,CategoryId")] BooksCategories booksCategories)
        {
            if (ModelState.IsValid)
            {
                db.BooksCategories.Add(booksCategories);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookId = new SelectList(db.Books, "BookID", "BookTitle", booksCategories.BookId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryID", "CategoryName", booksCategories.CategoryId);
            return View(booksCategories);
        }

        // GET: BooksCategories/Edit/5
        public ActionResult Edit(int? bookId, int? categoryId)
        {
            if (bookId == null && categoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BooksCategories booksCategories = db.BooksCategories.Find(bookId, categoryId);
            if (booksCategories == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookId = new SelectList(db.Books, "BookID", "BookTitle", booksCategories.BookId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryID", "CategoryName", booksCategories.CategoryId);
            return View(booksCategories);
        }

        // POST: BooksCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookId,CategoryId")] BooksCategories booksCategories)
        {
            if (ModelState.IsValid)
            {
                var services = db.BooksCategories.Where(b => b.BookId == booksCategories.BookId)
                                .Where(c => c.CategoryId == booksCategories.CategoryId);

                foreach(var item in services)
                {
                    db.BooksCategories.Remove(item);
                }
                db.BooksCategories.Add(booksCategories);
                db.Entry(booksCategories).State = EntityState.Modified;
                                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookId = new SelectList(db.Books, "BookID", "BookTitle", booksCategories.BookId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryID", "CategoryName", booksCategories.CategoryId);
            return View(booksCategories);
        }

        // GET: BooksCategories/Delete/5
        public ActionResult Delete(int? bookId, int? categoryId)
        {
            if (bookId == null || categoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BooksCategories booksCategories = db.BooksCategories.Find(bookId,categoryId);
            if (booksCategories == null)
            {
                return HttpNotFound();
            }
            return View(booksCategories);
        }

        // POST: BooksCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? bookId, int? categoryId)
        {
            BooksCategories booksCategories = db.BooksCategories.Find(bookId,categoryId);
            db.BooksCategories.Remove(booksCategories);
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
    }
}
