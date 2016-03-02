using System;
using System.Web.Mvc;
using TheGalleryWalk.Models;
using Parse;
using System.Threading.Tasks;

namespace TheGalleryWalk.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}