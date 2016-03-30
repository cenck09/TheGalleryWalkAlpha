using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Parse;
using TheGalleryWalk.Models;
using System.Threading.Tasks;

namespace TheGalleryWalk.Controllers
{
    public class OwnedArtistsController : AsyncController
    {      
        public async Task<ActionResult> OwnedArtists()
        {
            ViewBag.showForm = 0;
            var userInstance = ParseUser.CurrentUser;
            GalleryOwnerParseUser user = new GalleryOwnerParseUser().getInstanceFromParseObject(userInstance);

            if (this.verifyUser(userInstance))
            {
                return await baseView(user);
            }
            else{return returnFailedUserView();}
        }

        public ActionResult AddArtist()
        {
            return PartialView("~/Views/AddArtist/Index.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> AddArtist(ArtistEntity registerData)
        {
            if (!verifyUser(ParseUser.CurrentUser)){ return returnFailedUserView(); }

            ViewBag.showForm = 1;
            GalleryOwnerParseUser user = new GalleryOwnerParseUser().getInstanceFromParseObject(ParseUser.CurrentUser);

            if (ModelState.IsValid)
            {
                ArtistParseClass artist = new ArtistParseClass()
                {
                    Name = registerData.Name,
                    Style = registerData.Style,
                    Birth = registerData.Birth,
                    Death = registerData.Death,
                    Description = registerData.Description,
                    GalleryOwnerID = user.ObjectId
                };

                try
                {
                    await artist.SaveAsync();
                    ViewBag.showForm = 0;
                }
                catch (Exception ex){Debug.WriteLine(ex);}
            }       
            return await baseView(user);
        }

        public async Task<ActionResult> baseView(GalleryOwnerParseUser user)
        {
            GalleryOwnerEntity owner = user.toEntityWithSelf();
            try
            {
                var query = from item in new ParseQuery<ArtistParseClass>()
                            where item.GalleryOwnerID == user.ObjectId
                            select item;

                owner.ArtistEntities = await query.FindAsync();
            }
            catch (Exception ex)
            {
                owner.ArtistEntities = new List<ArtistParseClass>();
            }

            try
            {
                var query = new ParseQuery<ParseUser>().WhereContainedIn("objectId", user.MyFavoriteArtists);
                IEnumerable<ParseObject> favArtists = await query.FindAsync();

                if (owner.MyFavoriteArtists == null)
                {
                    owner.MyFavoriteArtists = new List<ArtistParseUser>();
                }
         
                Debug.WriteLine("\n\n ----- Fav Artists loaded array " + favArtists.Count());
                foreach (var item in favArtists)
                {
                    Debug.WriteLine("Processing item : " + item.ObjectId);
                    owner.MyFavoriteArtists.Add(new ArtistParseUser() {
                        ObjectId = item.ObjectId,
                        Name = item.Get<string>("Name")
                    });
                }
               // owner.FollowedArtists = await query.FindAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("There was an error :: " + ex);
            }

            return View("~/Views/OwnedArtists/OwnedArtists.cshtml", "_LayoutLoggedIn", owner);
        }

        public ActionResult returnFailedUserView()
        {
            return View("../Home/Index", "_Layout");
        }

        private bool verifyUser(ParseUser user)
        {
            if (user == null) { return false; }
            else if (!user.IsAuthenticated) { return false; }
            else { return true; }
        }

    }
}