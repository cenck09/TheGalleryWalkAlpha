using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Parse;
using TheGalleryWalk.Models;
using System.Diagnostics;

namespace TheGalleryWalk.Controllers
{
    public class GalleryController : AsyncController
    {

        public async Task<ActionResult> GalleryView(GalleryEntity selectedGallery)
        {
            ViewBag.showForm = 0;

            var user = ParseUser.CurrentUser;
            if(!verifyUser(user))
            {
                return returnFailedUserView();
            }
            else
            {
                return await baseView(user, selectedGallery);
            }
           
        }// EOM


        public ActionResult AddArtwork()
        {
            return PartialView("~/Views/AddArtwork/Index.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> AddArtwork(GalleryEntity registerData)
        {

            var galleryQuery = ParseObject.GetQuery("Gallery");
            galleryQuery = galleryQuery.WhereContainedIn("objectId", registerData.parseID);

            GalleryParseObject Gallery = null;
            IEnumerable<ParseObject> GalleryEntities = await galleryQuery.FindAsync();
            try
            {
                foreach (var item in GalleryEntities)
                {
                    Gallery = (item as GalleryParseObject);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                return returnFailedUserView();
            }
           

            var Entity = new ParseObject("Artwork");
            Entity.Add("Name", registerData.Name);

            ViewBag.showForm = 1;

            var user = ParseUser.CurrentUser;
            var Query = ParseObject.GetQuery("Artwork");

            IList<string> Ids;
            try
            {
                Ids = Gallery.Get<IList<string>>("Galleries");
            }
            catch
            {
                Ids = new List<string>();
            }



            if (ModelState.IsValid)
            {
                try
                {
                    IEnumerable<ParseObject> Entities = new List<ParseObject>();
                    await Entity.SaveAsync();

                    Ids.Add(Entity.ObjectId);
                    user["Galleries"] = Ids;
                
                    await user.SaveAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            return await baseView(user, registerData);
        }







        public async Task<ActionResult> baseView(ParseUser user, GalleryEntity selectedGallery)
        {
           
            IList<string> artworkIds; // = user.Get<IList<string>>("Galleries");
            try
            {
                artworkIds = user.Get<IList<string>>("Artwork");
            }
            catch
            {
                artworkIds = new List<string>();
                selectedGallery.Artworks = artworkIds;
            }

            IEnumerable<ParseObject> ArtworkEntities;
            var artworkQuery = ParseObject.GetQuery("Artwork");

            if (artworkIds.Count > 0)
            {
                artworkQuery = artworkQuery.WhereContainedIn("objectId", artworkIds);
                ArtworkEntities = await artworkQuery.FindAsync();
                selectedGallery.ArtworkEntities = ArtworkEntities;
            }

            return View("~/Views/Gallery/GalleryView.cshtml", "~/Views/Shared/_LayoutLoggedIn.cshtml", selectedGallery);
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