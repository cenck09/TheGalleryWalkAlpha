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
    public class OwnedArtworkController : AsyncController
    {      
        public async Task<ActionResult> OwnedArtwork()
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

        public ActionResult AddArtwork()
        {
            Debug.WriteLine("Add artwork function called");
            return PartialView("~/Views/AddArtwork/Index.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> AddArtwork(ArtworkEntity registerData)
        {
            var user = ParseUser.CurrentUser;
            if (!verifyUser(user))
            {
                return returnFailedUserView();
            }

            var artworkQuery = ParseObject.GetQuery("Artwork");
            var artworkEntity = new ParseObject("Artwork");
            artworkEntity.Add("Name", registerData.Name);
            artworkEntity.Add("Description", registerData.Description);
          

            ViewBag.showForm = 1;
        
            IList<string> artworkIds = user.Get<IList<string>>("Artwork");
         
            if (ModelState.IsValid)
            {
                try
                {
                    await artworkEntity.SaveAsync();

                    artworkIds.Add(artworkEntity.ObjectId);
                    user["Artwork"] = artworkIds;
                
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
            IList<string> artworkIds; // = user.Get<IList<string>>("Galleries");
            try
            {
                artworkIds = user.Get<IList<string>>("Artwork");
            }
            catch
            {
                artworkIds = new List<string>();
                user["Artwork"] = artworkIds;
                await user.SaveAsync();
            }

            IEnumerable<ParseObject> ArtworkEntities;
            var artworkQuery = ParseObject.GetQuery("Artwork");
            GalleryEntity G_Owner = new GalleryEntity();
            G_Owner.ArtworkAdd = new ArtworkEntity();

            if (artworkIds.Count > 0)
            {
                artworkQuery = artworkQuery.WhereContainedIn("objectId", artworkIds);

                ArtworkEntities = await artworkQuery.FindAsync();

                G_Owner.ArtworkEntities = ArtworkEntities;

                return View("~/Views/OwnedArtwork/OwnedArtwork.cshtml", "_LayoutLoggedIn", G_Owner);
            }

            return View("../OwnedArtwork/OwnedArtwork", "_LayoutLoggedIn");
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