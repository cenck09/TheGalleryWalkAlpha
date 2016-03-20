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
        public async Task<ActionResult> AddArtwork(ArtworkEntity registerData)
        {
            if (!verifyUser(ParseUser.CurrentUser))
            {
                return returnFailedUserView();
            }

            var galleryQuery = ParseObject.GetQuery("Gallery");
            galleryQuery = galleryQuery.WhereEqualTo("objectId", registerData.ParentGalleryParseID);

            ParseObject Gallery = null;
            try
            {
                Gallery = await galleryQuery.FirstAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
           

            var Entity = new ParseObject("Artwork");
            Entity.Add("Name", registerData.Name);

            ViewBag.showForm = 1;

            IList<string> Ids;
            try
            {
                Ids = Gallery.Get<IList<string>>("Artworks");
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                Ids = new List<string>();
            }



            if (ModelState.IsValid)
            {
                try
                {
                    await Entity.SaveAsync();

                    Ids.Add(Entity.ObjectId);
                    Gallery["Artworks"] = Ids;
                
                    await Gallery.SaveAsync();
                    ViewBag.showForm = 0;

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            GalleryEntity galleryToReturn = new GalleryEntity();
            galleryToReturn.Name = Gallery.Get<string>("Name");
            galleryToReturn.EmailAddress = Gallery.Get<string>("Email");
            galleryToReturn.phoneNumber = Gallery.Get<string>("PhoneNumber");
            try
            {
                galleryToReturn.Address = Gallery.Get<string>("Address");
            }catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            galleryToReturn.parseID = Gallery.ObjectId;
            galleryToReturn.Artworks = Ids;
            galleryToReturn.ArtworkAdd = registerData;

            return await baseView(ParseUser.CurrentUser, galleryToReturn);
        }







        public async Task<ActionResult> baseView(ParseUser user, GalleryEntity selectedGallery)
        {
            var galleryQuery = ParseObject.GetQuery("Gallery");
            galleryQuery = galleryQuery.WhereEqualTo("objectId", selectedGallery.parseID);

            ParseObject Gallery = null;
            try
            {
                Gallery = await galleryQuery.FirstAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            IList<string> artworkIds; // = user.Get<IList<string>>("Galleries");
            try
            {
                artworkIds = Gallery.Get<IList<string>>("Artworks");
            }
            catch(Exception ex)
            {
                artworkIds = new List<string>();
                Debug.WriteLine(ex);
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