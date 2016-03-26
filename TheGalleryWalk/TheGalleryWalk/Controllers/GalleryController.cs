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
            if (!verifyUser(ParseUser.CurrentUser))
            {
                return returnFailedUserView();
            }

            ArtworkParseClass artwork = new ArtworkParseClass()
            {
                Name = registerData.Name,
                ArtistID = registerData.Artist
            };

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
            return await baseView(gallery);
        }


    
      

        public async Task<ActionResult> baseView(GalleryEntity G)
        {
            if (G.ArtworkAdd == null) { G.ArtworkAdd = new ArtworkEntity(); }

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
                array.Add(item.ObjectId);
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








/*
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
         galleryToReturn.ArtistAdd = registerData;
        galleryToReturn.ArtworkAdd = new ArtworkEntity();
        galleryToReturn.ArtworkAdd.ParentGalleryParseID = Gallery.ObjectId;

        return await baseView(ParseUser.CurrentUser, galleryToReturn);
    }


    */
