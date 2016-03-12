using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheGalleryWalk.Controllers
{
    public class OwnedGalleriesController : AsyncController
    {
        protected void Page_Init(object sender, EventArgs e)
        {

            Debug.WriteLine("Page Init Called");

        }

        // GET: OwnedGalleries
        public ActionResult OwnedGalleries()
        {
            Debug.WriteLine("Loading Owned Galleries View");
            return View();
        }
    }
}