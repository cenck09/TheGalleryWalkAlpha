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
    public class OwnedArtistsController : AsyncController
    {      
        public async Task<ActionResult> OwnedArtists()
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

        public ActionResult AddArtist()
        {
            Debug.WriteLine("Add artist function called");
            return PartialView("~/Views/AddArtist/Index.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> AddArtist(ArtistEntity registerData)
        {
            var user = ParseUser.CurrentUser;
            if (!verifyUser(user))
            {
                return returnFailedUserView();
            }

            var artistQuery = ParseObject.GetQuery("Artist");
            var artistEntity = new ParseObject("Artist");
            artistEntity.Add("Name", registerData.Name);
            artistEntity.Add("Style", registerData.Style);
            artistEntity.Add("Artworks", new List<string>());

            ViewBag.showForm = 1;
        
            IList<string> artistIds = user.Get<IList<string>>("Artists");
         
            if (ModelState.IsValid)
            {
                try
                {
                    await artistEntity.SaveAsync();

                    artistIds.Add(artistEntity.ObjectId);
                    user["Artists"] = artistIds;
                
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
            IList<string> artistIds; // = user.Get<IList<string>>("Galleries");
            try
            {
                artistIds = user.Get<IList<string>>("Artists");
            }
            catch
            {
                artistIds = new List<string>();
                user["Artists"] = artistIds;
                await user.SaveAsync();
            }

            IEnumerable<ParseObject> ArtistEntities;
            var artistQuery = ParseObject.GetQuery("Artist");
            GalleryEntity G_Owner = new GalleryEntity();
            G_Owner.ArtistAdd = new ArtistEntity();

            if (artistIds.Count > 0)
            {
                artistQuery = artistQuery.WhereContainedIn("objectId", artistIds);

                ArtistEntities = await artistQuery.FindAsync();

               // G_Owner.ArtistEntities = ArtistEntities;

                return View("~/Views/OwnedArtists/OwnedArtists.cshtml", "_LayoutLoggedIn", G_Owner);
            }

            return View("../OwnedArtists/OwnedArtists", "_LayoutLoggedIn");
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