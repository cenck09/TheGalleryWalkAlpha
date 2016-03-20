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
                return returnFailedUserView();

            } else if (user.IsAuthenticated)
            {
                return await returnBaseView(user);
            }
            else
            {
                return returnFailedUserView();
            }
        }

        public ActionResult AddGallery()
        {
            Debug.WriteLine("Add gallery function called");
            return PartialView("~/Views/AddGallery/Index.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> AddGallery(GalleryEntity registerData)
        {
            var user = ParseUser.CurrentUser;
            if (!verifyUser(user))
            {
                return returnFailedUserView();
            }

            var galleryQuery = ParseObject.GetQuery("Gallery");
            var galleryEntity = new ParseObject("Gallery");
            galleryEntity.Add("Name", registerData.Name);
            galleryEntity.Add("Email", registerData.EmailAddress);
            galleryEntity.Add("Address", registerData.Address);
            galleryEntity.Add("PhoneNumber", registerData.phoneNumber);
            galleryEntity.Add("Artists", new List<string>());
            galleryEntity.Add("Artworks", new List<string>());

            ViewBag.showForm = 1;
        
            IList<string> galleryIds = user.Get<IList<string>>("Galleries");
         
            if (ModelState.IsValid)
            {
                try
                {
                    await galleryEntity.SaveAsync();

                    galleryIds.Add(galleryEntity.ObjectId);
                    user["Galleries"] = galleryIds;
                
                    await user.SaveAsync();
                    ViewBag.showForm = 0;

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            return await returnBaseView(user);
        }

        public async Task<ActionResult> returnBaseView(ParseUser user)
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

        public ActionResult returnFailedUserView()
        {
            return View("../Home/Index", "_Layout");
        }

        private bool verifyUser(ParseUser user)
        {
            if (user == null)
            {
                return false;

            }
            else if (!user.IsAuthenticated)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}