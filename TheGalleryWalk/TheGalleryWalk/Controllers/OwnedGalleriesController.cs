using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Parse;
using TheGalleryWalk.Models;

namespace TheGalleryWalk.Controllers
{
    public class OwnedGalleriesController : AsyncController
    {
      

        // GET: OwnedGalleries
        public ActionResult OwnedGalleries()
        {
            var user = ParseUser.CurrentUser;
            if (user == null)
            {
                return View("../Home/Index", "_Layout");
            }else if (user.IsAuthenticated)
            {
                var ownerData = new GalleryOwnerData();
                ownerData.EmailAddress = user.Email;
                return View("../OwnedGalleries/OwnedGalleries", "_LayoutLoggedIn", ownerData);
            }
            else
            {
                return View("../Home/Index", "_Layout");
            }
        }
    }
}