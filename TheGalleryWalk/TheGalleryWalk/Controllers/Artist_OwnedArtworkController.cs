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
    public class Artist_OwnedArtworkController : Controller
    {
        // GET: Artist_OwnedArtwork
        public async Task<ActionResult> Artist_OwnedArtwork()
        {
            ViewBag.showForm = 0;
            var user = ParseUser.CurrentUser;

            if (this.verifyUser(user))
            {
                ArtistParseUser ParseUser = new ArtistParseUser().getInstanceFromParseObject(user);
                return await returnBaseView(ParseUser.toEntityWithSelf());
            }
            else
            {
                return returnFailedUserView();
            }
        }

        public ActionResult AddArtwork()
        {
            return PartialView("~/Views/AddArtwork_Artist/Index.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> AddArtwork(ArtworkEntity registerData)
        {
            var userInstance = ParseUser.CurrentUser;

            ArtistParseUser user = new ArtistParseUser().getInstanceFromParseObject(userInstance);

            var artistUser = user.toEntityWithSelf();

            if (!verifyUser(userInstance))
            {
                return returnFailedUserView();
            }

            ViewBag.showForm = 1;

            if (ModelState.IsValid)
            {
                var Entity = new ArtworkParseClass()
                {
                    Name = registerData.Name,
                    GalleryID = null,
                    ArtistID = user.ObjectId,
                    Description = registerData.Description
                };


                try
                {
                    await Entity.SaveAsync();
                    Debug.WriteLine("OWNER ID ON SAVED GALLERY OBJECT :: " + Entity.ArtistID);
                    ViewBag.showForm = 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            else
            {
                artistUser.ArtworkAdd = registerData;
            }

            return await returnBaseView(artistUser);
        }


        public async Task<ActionResult> returnBaseView(ArtistUserEntity G_Owner)
        {
            if (G_Owner.ArtworkAdd == null) { G_Owner.ArtworkAdd = new ArtworkEntity(); }

            try
            {
                var query = from item in new ParseQuery<ArtworkParseClass>()
                            where item.ArtistID == G_Owner.ParseID
                            select item;

                G_Owner.ArtworkEntities = await query.FindAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("There was an error returning owned galleries base view :: " + ex);
                G_Owner.ArtworkEntities = new List<ArtworkParseClass>();
            }


            return View("~/Views/Artist_OwnedArtwork/OwnedArtwork.cshtml", "_LayoutArtistLoggedIn", G_Owner);
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