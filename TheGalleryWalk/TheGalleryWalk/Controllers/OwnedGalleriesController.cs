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

        // GET: OwnedGalleries
        public async Task<ActionResult> OwnedGalleries()
        {
            var user = ParseUser.CurrentUser;
            if (user == null)
            {
                return View("../Home/Index", "_Layout");
            }
            else if (user.IsAuthenticated)
            {
                List<string> galleryIds;// = user.Get<List<string>>("Galleries");

                try
                {
                    galleryIds = user.Get<List<string>>("Galleries");
                    Debug.WriteLine("Post load galleries id array");
                    Debug.WriteLine(galleryIds);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    galleryIds = new List<string>();
                    Debug.WriteLine("Failed to load a list of Id's");
                }

                IEnumerable<ParseObject> GalleryEntities;

                var galleryQuery = ParseObject.GetQuery("Gallery");
                if (galleryIds.Count > 0)
                {
                   
                    Debug.WriteLine("Proc ID ", galleryIds.ToString());
                    galleryQuery = galleryQuery.WhereContainedIn("ObjectId", galleryIds);
                    
                    GalleryEntities = await galleryQuery.FindAsync();
                    var GE = GalleryEntities.ToArray<ParseObject>();


                    for(var i = 0; i < GalleryEntities.Count(); i++)
                    {
                       
                    }
                    GalleryOwnerEntity owner = new GalleryOwnerEntity();
                    Debug.WriteLine("Before loading page with entities");
                    ViewBag.Data = GalleryEntities;

                    return View("../OwnedGalleries/OwnedGalleries", "_LayoutLoggedIn");
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
        public async Task<ActionResult> AddGallery(GalleryOwnerEntity registerData)
        {
            var galleryEntity = new ParseObject("Gallery");
            galleryEntity.Add("Name", registerData.Name);
            galleryEntity.Add("Email", registerData.EmailAddress);
            galleryEntity.Add("Address", registerData.Address);

            // other fields can be set just like with ParseObject
            try
            {
                await galleryEntity.SaveAsync();
                var user = ParseUser.CurrentUser;

                IList<string> galleryIds;// = user.Get<List<string>>("Galleries");

                try
                {
                    galleryIds = user.Get<List<string>>("Galleries");

                }
                catch (Exception ex)
                {

                    galleryIds = new List<string>();
                }


                galleryIds.Add(galleryEntity.ObjectId);
                Debug.WriteLine(galleryIds.ToString());
                user["Galleries"] = galleryIds;
              //  user.Add("Galleries", galleryIds);
                await user.SaveAsync();

                Debug.WriteLine("Post add gallery");
                return View("~/Views/OwnedGalleries/OwnedGalleries.cshtml");

            }
            catch (Exception ex)
            {
                Debug.WriteLine("There was an error " + ex);
                return View("~/Views/AddGallery/Index.cshtml");
            }
        }
    }
}