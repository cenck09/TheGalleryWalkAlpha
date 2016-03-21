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
    public class ArtistController : AsyncController
    {

        public async Task<ActionResult> ArtistView(ArtistEntity selectedArtist)
        {
             ViewBag.showForm = 0;

            var user = ParseUser.CurrentUser;
            if(!verifyUser(user))
            {
                return returnFailedUserView();
            }
            else
            {
                return await baseView(user, selectedArtist);
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
                Debug.WriteLine("EXCEPTION GETTING GALLERY FOR ADD ARTWORK FIRSTASYNC "+ex);
            }
           

            var Entity = new ParseObject("Artwork");
            Entity.Add("Name", registerData.Name);
            Entity.Add("Artist", registerData.Artist);
            Entity.Add("Description", registerData.Description);

            Debug.WriteLine("ARTWORK ENTITY ARTIST ID =  " + Entity.Get<string>("Artist"));

            ViewBag.showForm = 1;

            IList<string> Ids;
            try
            {
                Ids = Gallery.Get<IList<string>>("Artworks");
            }
            catch(Exception ex)
            {
                Debug.WriteLine("EXCEPTION GETTING GALLERY ARTWORK LIST "+ex);
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
                    Debug.WriteLine("Failed to add Artwork "+ex);
                }
            }
            GalleryEntity galleryToReturn = getGalleryEntityForParseObject(Gallery);
            galleryToReturn.parseID = Gallery.ObjectId;
            galleryToReturn.Artworks = Ids;
            galleryToReturn.ArtworkAdd = registerData;
            galleryToReturn.ArtistAdd = new ArtistEntity();
            galleryToReturn.ArtistAdd.ParentGalleryParseID = Gallery.ObjectId;
            galleryToReturn.ArtworkAdd.ParentGalleryParseID = Gallery.ObjectId;

            return await baseView(ParseUser.CurrentUser, galleryToReturn);
        }



        public ActionResult AddArtist()
        {
            return PartialView("~/Views/AddArtist/Index.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> AddArtist(ArtistEntity registerData)
        {
            if (!verifyUser(ParseUser.CurrentUser))
            {
                return returnFailedUserView();
            }

            var galleryQuery = ParseObject.GetQuery("Gallery");
            galleryQuery = galleryQuery.WhereEqualTo("objectId", registerData.ParentGalleryParseID);
            ViewBag.showForm = -1;

            ParseObject Gallery = null;
            try
            {
                Gallery = await galleryQuery.FirstAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }


            var Entity = new ParseObject("Artist");
            Entity.Add("Name", registerData.Name);
            Entity.Add("Style", registerData.Style);
            try
            {
                Entity.Add("Description", registerData.Description);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

            }


            IList<string> Ids;
            try
            {
                Ids = Gallery.Get<IList<string>>("Artists");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Ids = new List<string>();
            }

            IList<string> userIds;
            try
            {
                userIds = ParseUser.CurrentUser.Get<IList<string>>("Artists");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                userIds = new List<string>();
            }



            if (ModelState.IsValid)
            {
                try
                {
                    await Entity.SaveAsync();

                    Ids.Add(Entity.ObjectId);
                    userIds.Add(Entity.ObjectId);

                    Gallery["Artists"] = Ids;

                    await Gallery.SaveAsync();
                    ParseUser user = ParseUser.CurrentUser;

                    user["Artists"] = userIds;
                    await user.SaveAsync();
                    ViewBag.showForm = 0;
                    Debug.WriteLine("Artist has been saved!");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            GalleryEntity galleryToReturn = getGalleryEntityForParseObject(Gallery);
            galleryToReturn.parseID = Gallery.ObjectId;
            galleryToReturn.Artists = Ids;
            galleryToReturn.ArtistAdd = registerData;
            galleryToReturn.ArtworkAdd = new ArtworkEntity();
            galleryToReturn.ArtworkAdd.ParentGalleryParseID = Gallery.ObjectId;

            return await baseView(ParseUser.CurrentUser, galleryToReturn);
        }




        public GalleryEntity getGalleryEntityForParseObject(ParseObject Gallery)
        {
            GalleryEntity galleryToReturn = new GalleryEntity();
            galleryToReturn.Name = Gallery.Get<string>("Name");
            galleryToReturn.EmailAddress = Gallery.Get<string>("Email");
            galleryToReturn.phoneNumber = Gallery.Get<string>("PhoneNumber");
            try
            {
                galleryToReturn.Artists = Gallery.Get<IList<string>>("Artists");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get artist id list from gallery parse object : " + ex);
                galleryToReturn.Artists = new List<string>();
            }
            try
            {
                galleryToReturn.Artworks = Gallery.Get<IList<string>>("Artworks");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get artist id list from gallery parse object : " + ex);
                galleryToReturn.Artworks = new List<string>();
            }
            try
            {
                galleryToReturn.Address = Gallery.Get<string>("Address");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get address "+ ex);
            }

            return galleryToReturn;
        }


        public async Task<ActionResult> baseView(ParseUser user, GalleryEntity selectedGallery)
        {
            var galleryQuery = ParseObject.GetQuery("Gallery");
            var userArtistQuery = ParseObject.GetQuery("Artist");
            galleryQuery = galleryQuery.WhereEqualTo("objectId", selectedGallery.parseID);


            IList<string> userArtworkIds; // = user.Get<IList<string>>("Galleries");
            try
            {
                userArtworkIds = user.Get<IList<string>>("Artists");
            }
            catch (Exception ex)
            {
                userArtworkIds = new List<string>();
                Debug.WriteLine("Failed to get artist list from user "+ex);
            }
            userArtistQuery = userArtistQuery.WhereContainedIn("objectId", userArtworkIds);

            IEnumerable<ParseObject> userArtistEntities;
            try
            {
                userArtistEntities = await userArtistQuery.FindAsync();
            }catch(Exception ex)
            {
                userArtistEntities = new List<ParseObject>();
                Debug.WriteLine("Failed to get artist entities : "+ex);
            }

            if (selectedGallery.ArtworkAdd == null)
            {
              
                selectedGallery.ArtworkAdd = new ArtworkEntity();
           
                selectedGallery.ArtworkAdd.ParentGalleryParseID = selectedGallery.parseID;

            }

            selectedGallery.ArtworkAdd.addArtistFormlistItem = new List<SelectListItem>();

            if (userArtistEntities.Count() == 0)
            {
                Debug.WriteLine("User Artist count is 0!");
            }
            foreach (ParseObject item in userArtistEntities)
            {
                Debug.WriteLine("adding artist to list :: " + item.Get<string>("Name"));
                selectedGallery.ArtworkAdd.addArtistFormlistItem.Add(new SelectListItem
                {
                    Text = item.Get<string>("Name"),
                    Value = item.ObjectId
                });
            }

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

            IList<string> artistIds; // = user.Get<IList<string>>("Galleries");
            try
            {
                artistIds = Gallery.Get<IList<string>>("Artists");
            }
            catch (Exception ex)
            {
                artistIds = new List<string>();
                Debug.WriteLine(ex);
            }

            IEnumerable<ParseObject> ArtistEntities;
            var artistQuery = ParseObject.GetQuery("Artist");

            if (artistIds.Count > 0)
            {
                artistQuery = artistQuery.WhereContainedIn("objectId", artistIds);
                ArtistEntities = await artistQuery.FindAsync();
                selectedGallery.ArtistEntities = ArtistEntities;
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