using System;
using System.Web.Mvc;
using TheGalleryWalk.Models;
using Parse;
using System.Threading.Tasks;

namespace TheGalleryWalk.Controllers
{
    public class HomeController : Controller
    {
        public bool verifyUser()
        {
            var user = ParseUser.CurrentUser;
            if (user == null)
            {
                return false;
            }
            if (user.IsAuthenticated)
            {
                return true;
            } else {return false;  }
        }
        public ActionResult Index()
        {
           
            if (this.verifyUser())
            {
                return View("../Home/Index", "_LayoutLoggedIn");
            }
            else
            {
                return View("../Home/Index", "_Layout");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            if (this.verifyUser())
            {
                return View("../Home/About", "_LayoutLoggedIn");
            }
            else
            {
                return View("../Home/About", "_Layout");
            }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            if(this.verifyUser())
            {
                return View("../Home/Contact", "_LayoutLoggedIn");
            }
            else
            {
                return View("../Home/Contact", "_Layout");
            }
        }
    }
}