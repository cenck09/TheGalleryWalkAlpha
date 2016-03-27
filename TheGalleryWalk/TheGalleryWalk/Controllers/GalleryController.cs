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
                return await baseView(selectedGallery);
            }
       }// EOM


        public ActionResult AddArtwork()
        {
            return PartialView("~/Views/AddArtwork/Index.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> AddArtwork(ArtworkEntity registerData)
        {
            ViewBag.showForm = 1;
            if (!verifyUser(ParseUser.CurrentUser))
            {
                return returnFailedUserView();
            }

            GalleryEntity gallery = new GalleryEntity();
            try
            {
                var query = from item in new ParseQuery<GalleryParseClass>()
                            where item.ObjectId == registerData.ParentGalleryParseID
                            select item;

                GalleryParseClass gClass = await query.FirstAsync();
                gallery = gClass.toEntityWithSelf();
            }

            catch (Exception ex)
            {
                Debug.WriteLine("There was an error returning owned galleries base view :: " + ex);
                returnFailedUserView();
            }

            if (ModelState.IsValid)
            {
                ArtworkParseClass artwork = new ArtworkParseClass()
                {
                    Name = registerData.Name,
                    ArtistID = registerData.Artist,
                    Description = registerData.Description,
                    GalleryID = registerData.ParentGalleryParseID
                };

                await artwork.SaveAsync();
                ViewBag.showForm = 0;
            }

            return await baseView(gallery);
        }
  

        public async Task<ActionResult> baseView(GalleryEntity G)
        {
            try
            {
                var query = from item in new ParseQuery<ArtworkParseClass>()
                            where item.GalleryID == G.ParseID
                            select item;

                G.ArtworkEntities = await query.FindAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("There was an error returning owned galleries base view :: " + ex);
                G.ArtworkEntities = new List<ArtworkParseClass>();
            }

            var array = new List<string>();

            foreach (var item in G.ArtworkEntities){
                array.Add(item.ArtistID);
            }

            try
            {
                var query = from item in new ParseQuery<ArtistParseClass>()
                            where array.Contains(item.ObjectId)
                            select item;

                G.ArtistEntities = await query.FindAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("There was an error returning owned galleries base view :: " + ex);
                G.ArtistEntities = new List<ArtistParseClass>();
            }

            if (G.ArtworkAdd == null)
            {
                G.ArtworkAdd = new ArtworkEntity();
                G.ArtworkAdd.addArtworkFormlistItem = new List<SelectListItem>();

                GalleryOwnerEntity owner = new GalleryOwnerParseUser().getInstanceFromParseObject(ParseUser.CurrentUser).toEntityWithSelf();

                try
                {
                    var query = from item in new ParseQuery<ArtistParseClass>()
                                where item.GalleryOwnerID == owner.ParseID
                                select item;

                    owner.ArtistEntities = await query.FindAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("There was an error returning owned galleries base view :: " + ex);
                    owner.ArtistEntities = new List<ArtistParseClass>();
                }

                foreach (ParseObject item in owner.ArtistEntities)
                {
                    Debug.WriteLine("adding artist to list :: " + item.Get<string>("Name"));
                    G.ArtworkAdd.addArtworkFormlistItem.Add(new SelectListItem
                    {
                        Text = item.Get<string>("Name"),
                        Value = item.ObjectId
                    });
                }
            }
            return View("~/Views/Gallery/GalleryView.cshtml", "~/Views/Shared/_LayoutLoggedIn.cshtml", G );
        }

        public ActionResult returnFailedUserView()
        {
            return View("../Home/Index", "_Layout");
        }

        private bool verifyUser(ParseUser user)
        {
            if (user == null) { return false; }
            else if (!user.IsAuthenticated){ return false; }
            else { return true; }
        }

    }
}

