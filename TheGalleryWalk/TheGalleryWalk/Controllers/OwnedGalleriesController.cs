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

                ownerData.galleries = new GalleryEntity[5];
                ownerData.galleries[0] = new GalleryEntity();
                ownerData.galleries[0].Name = "G0";

                ownerData.galleries[1] = new GalleryEntity();
                ownerData.galleries[1].Name = "G1";

                ownerData.galleries[2] = new GalleryEntity();
                ownerData.galleries[2].Name = "G2";

                ownerData.galleries[3] = new GalleryEntity();
                ownerData.galleries[3].Name = "G3";

                ownerData.galleries[4] = new GalleryEntity();
                ownerData.galleries[4].Name = "G4";

                return View("../OwnedGalleries/OwnedGalleries", "_LayoutLoggedIn", ownerData);
            }
            else
            {
                return View("../Home/Index", "_Layout");
            }
        }
    }
}