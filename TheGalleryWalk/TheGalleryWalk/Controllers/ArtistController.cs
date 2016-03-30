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


        public ActionResult updateGalleryInfo()
        {
            return PartialView("~/Views/EditArtist/EditArtistPartialView.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> updateArtistInfo(ArtistEntity artist)
        {
            if (!verifyUser(ParseUser.CurrentUser))
            {
                return returnFailedUserView();
            }

            ViewBag.showForm = 2;
            if (ModelState.IsValid)
            {
                var query = from item in new ParseQuery<ArtistParseClass>()
                            where item.ObjectId == artist.parseID
                            select item;

                ArtistParseClass gClass = await query.FirstAsync();

                gClass.Name = artist.Name;
                gClass.Style = artist.Style;
                gClass.Description = artist.Description;

                try
                {
                    await gClass.SaveAsync();
                    ViewBag.showForm = 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            return await baseView(artist);
        }

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
                return await baseView( selectedArtist);
            }
           
        }// EOM
     
    
        


        public ArtistEntity getArtistEntityForParseObject(ParseObject Artist)
        {
            ArtistEntity artistToReturn = new ArtistEntity();
            artistToReturn.Name = Artist.Get<string>("Name");
            artistToReturn.Style = Artist.Get<string>("Style");
            artistToReturn.Description = Artist.Get<string>("Description");
            

            return artistToReturn;
        }





        public async Task<ActionResult> baseView(ArtistEntity artistUser)
        {
            try
            {
                var query = from item in new ParseQuery<ArtworkParseClass>()
                            where item.ArtistID == artistUser.parseID
                            select item;

                artistUser.ArtworkEntities = await query.FindAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("There was an error returning owned galleries base view :: " + ex);
                artistUser.ArtworkEntities = new List<ArtworkParseClass>();
            }


            if (this.verifyUser(ParseUser.CurrentUser))
            {
                if ("GalleryOwnerUser".Equals(ParseUser.CurrentUser.Get<string>("UserType")))
                {
                    return View("~/Views/Artist/ArtistView.cshtml", "~/Views/Shared/_LayoutLoggedIn.cshtml", artistUser);
                }
                else
                {
                    return returnFailedUserView();
                }
            }
            else
            {
                return returnFailedUserView();
            }
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