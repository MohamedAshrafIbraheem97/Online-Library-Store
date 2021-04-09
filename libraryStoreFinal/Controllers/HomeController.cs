using libraryStoreFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace libraryStoreFinal.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            
            
            return View(db.Books.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "The Library Store New Version";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Our Contact Page";

            return View();
        }
    }
}