using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheGalleryWalk.Controllers
{
    public class OwnedGalleriesPartialViewController : AsyncController
    {
        // GET: OwnedGalleriesPartialView
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OwnedGalleriesPartialView()
        {
            
            return PartialView();
        }
    }

}