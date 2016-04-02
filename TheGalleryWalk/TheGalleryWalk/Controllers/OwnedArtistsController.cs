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
    public class OwnedArtistsController : BaseValidatorController
    {      
        public async Task<ActionResult> OwnedArtists()
        {
            if (verifyUser())
            {
                ViewBag.showForm = 0;
                GeneralParseUserData userData = await getUserData();

                GalleryOwnerEntity owner = getGalleryOwnerEntity(userData);

                return await baseView(owner, userData);
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
            if (!verifyUser()){ return returnFailedUserView(); }


            ViewBag.showForm = 1;
            GeneralParseUserData userData = await getUserData();

            GalleryOwnerEntity owner = getGalleryOwnerEntity(userData);

            if (ModelState.IsValid)
            {
                ArtistParseClass artist = new ArtistParseClass()
                {
                    Name = registerData.Name,
                    Style = registerData.Style,
                    Birth = registerData.Birth,
                    Death = registerData.Death,
                    Description = registerData.Description,
                    GalleryOwnerID = getUserId(),
                    FileOwnerId = getUserId(),
                };

                try
                {
                    await artist.SaveAsync();
                    ViewBag.showForm = 0;
                }
                catch (Exception ex){Debug.WriteLine(ex);}
            }       
            return await baseView(owner, userData);
        }

        public async Task<ActionResult> baseView(GalleryOwnerEntity owner, GeneralParseUserData userData)
        {
          
            try
            {
                var query = from item in new ParseQuery<ArtistParseClass>()
                            where item.GalleryOwnerID == getUserId()
                            select item;

                foreach (ArtistParseClass artist in await query.FindAsync())
                {
                    owner.ArtistEntities.Add(getArtistEntity(artist));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                owner.ArtistEntities = new List<ArtistEntity>();
            }

            try
            {
                var query = new ParseQuery<GeneralParseUserData>().WhereContainedIn("UserId", userData.MyFavoriteArtists);
                IEnumerable<ParseObject> favArtists = await query.FindAsync();

                if (owner.MyFavoriteArtists == null)
                {
                    owner.MyFavoriteArtists = new List<ArtistUserEntity>();
                }
         
                Debug.WriteLine("\n\n ----- Fav Artists loaded array " + favArtists.Count());
                foreach (GeneralParseUserData item in favArtists)
                {
                    Debug.WriteLine("Processing item : " + item.ObjectId);
                    owner.MyFavoriteArtists.Add(getArtistUserEntity(item));
                }
               // owner.FollowedArtists = await query.FindAsync();
            }

            catch (Exception ex)
            {
                Debug.WriteLine("There was an error :: " + ex);
            }

            return View("~/Views/OwnedArtists/OwnedArtists.cshtml", "_LayoutLoggedIn", owner);
        }
    }
}