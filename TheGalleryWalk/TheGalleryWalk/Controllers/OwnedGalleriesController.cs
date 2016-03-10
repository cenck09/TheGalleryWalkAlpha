using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheGalleryWalk.Controllers
{
    public class OwnedGalleriesController : Controller
    {
        // GET: OwnedGalleries
        public ActionResult OwnedGalleries()
        {
            return View();
        }
    }
}