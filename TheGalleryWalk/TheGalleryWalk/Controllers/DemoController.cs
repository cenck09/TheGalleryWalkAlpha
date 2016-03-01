using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheGalleryWalk.Controllers
{
    public class DemoController : Controller
    {
        // GET: Demo
     
   public ActionResult Index()
        {
            // This method looks for an html layout in Views/Demo/Index.cshtml
            return View();
        }
    public ActionResult Demo()
    {
        ViewBag.Message = "A Demo page.";

        return View();
    }

   
    }

}