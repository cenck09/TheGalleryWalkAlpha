using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Parse;
using TheGalleryWalk.Models;
using System.Threading.Tasks;

namespace TheGalleryWalk.Controllers
{
    public class OwnedGalleriesController : AsyncController
    {



        public async Task<ActionResult> OwnedGalleries()
        {
            ViewBag.showForm = 0;

            var user = ParseUser.CurrentUser;
            if (user == null)
            {
                return View("../Home/Index", "_Layout");

            } else if (user.IsAuthenticated)
            {

                IList<string> galleryIds; // = user.Get<IList<string>>("Galleries");
                try
                {
                    galleryIds = user.Get<IList<string>>("Galleries");
                }
                catch
                {
                    galleryIds = new List<string>();
                    user["Galleries"] = galleryIds;
                    await user.SaveAsync();
                }

                IEnumerable<ParseObject> GalleryEntities;
                var galleryQuery = ParseObject.GetQuery("Gallery");
                GalleryOwnerEntity G_Owner = new GalleryOwnerEntity();
                G_Owner.GalleryAdd = new GalleryEntity();

                if (galleryIds.Count > 0)
                {
                    galleryQuery = galleryQuery.WhereContainedIn("objectId", galleryIds);

                    GalleryEntities = await galleryQuery.FindAsync();

                    G_Owner.GalleryEntities = GalleryEntities;

                    return View("~/Views/OwnedGalleries/OwnedGalleries.cshtml", "_LayoutLoggedIn", G_Owner);
                }

                return View("../OwnedGalleries/OwnedGalleries", "_LayoutLoggedIn");
            }
            else
            {
                return View("../Home/Index", "_Layout");
            }
        }


        public ActionResult AddGallery()
        {
            Debug.WriteLine("Add gallery function called");
            return PartialView("~/Views/AddGallery/Index.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> OwnedGalleries(GalleryEntity registerData)
        {
            var galleryEntity = new ParseObject("Gallery");
            galleryEntity.Add("Name", registerData.Name);
            galleryEntity.Add("Email", registerData.EmailAddress);
            galleryEntity.Add("Address", registerData.Address);

            ViewBag.showForm = 1;

            var user = ParseUser.CurrentUser;
            var galleryQuery = ParseObject.GetQuery("Gallery");

            IList<string> galleryIds;// = user.Get<IList<string>>("Galleries");

            try
            {
                galleryIds = user.Get<IList<string>>("Galleries");
            }
            catch
            {
                galleryIds = new List<string>();

            }


            IEnumerable<ParseObject> GalleryEntities;

            GalleryOwnerEntity G_Owner = new GalleryOwnerEntity();
            G_Owner.GalleryAdd = registerData;

            if (ModelState.IsValid)
            {
                try
                {
                    await galleryEntity.SaveAsync();

                    galleryIds.Add(galleryEntity.ObjectId);
                    user["Galleries"] = galleryIds;

                    await user.SaveAsync();

                    galleryQuery = galleryQuery.WhereContainedIn("objectId", galleryIds);
                    GalleryEntities = await galleryQuery.FindAsync();

                    G_Owner.GalleryAdd.Name = "";
                    G_Owner.GalleryAdd.EmailAddress = "";
                    G_Owner.GalleryAdd.phoneNumber = "";

                    G_Owner.GalleryEntities = GalleryEntities;

                    ViewBag.showForm = 0;

                    return View("~/Views/OwnedGalleries/OwnedGalleries.cshtml", "_LayoutLoggedIn", G_Owner );

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("There was an error " + ex);

                    if (galleryIds.Count > 0)
                    {
                        galleryQuery = galleryQuery.WhereContainedIn("objectId", galleryIds);
                        GalleryEntities = await galleryQuery.FindAsync();
                        G_Owner.GalleryEntities = GalleryEntities;

                        return View("~/Views/OwnedGalleries/OwnedGalleries.cshtml", "_LayoutLoggedIn", G_Owner);
                    }
                    else
                    {
                        return View("~/Views/OwnedGalleries/OwnedGalleries.cshtml", "_LayoutLoggedIn", G_Owner);
                    }
                }
            }
            else
            {
                if (galleryIds.Count > 0)
                {
                    galleryQuery = galleryQuery.WhereContainedIn("objectId", galleryIds);
                    GalleryEntities = await galleryQuery.FindAsync();

                    G_Owner.GalleryEntities = GalleryEntities;
                    return View("~/Views/OwnedGalleries/OwnedGalleries.cshtml", "_LayoutLoggedIn", G_Owner);
                }
                else
                {
                    return View("~/Views/OwnedGalleries/OwnedGalleries.cshtml", "_LayoutLoggedIn", G_Owner);
                }
            }
        }

    }
}