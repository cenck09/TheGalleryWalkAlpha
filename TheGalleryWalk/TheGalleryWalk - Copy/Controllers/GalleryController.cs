using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Parse;
using TheGalleryWalk.Models;
using System.Diagnostics;
using System.IO;

namespace TheGalleryWalk.Controllers
{
    public class GalleryController : BaseValidatorController
    {

        public async Task<ActionResult> GalleryView(GalleryEntity selectedGallery)
        {
             ViewBag.showForm = 0;

            if(!verifyUser())
            {
                return returnFailedUserView();
            }
            else
            {
                return await baseView(selectedGallery);
            }
       }// EOM

        public ActionResult updateGalleryInfo()
        {
            return PartialView("~/Views/EditGallery/EditGalleryPartialView.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> updateGalleryInfo(GalleryEntity gallery)
        {
            ViewBag.showForm = 2;
            if (ModelState.IsValid)
            {
                var query = from item in new ParseQuery<GalleryParseClass>()
                            where item.ObjectId == gallery.ParseID
                            select item;

                GalleryParseClass gClass = await query.FirstAsync();
                if (gClass.FileOwnerId != getUserId())
                {
                   return returnFailedUserView();
                }
                var array = new List<string>();
                gClass.Name = gallery.Name;
                gClass.Address = gallery.Address;
                gClass.Email = gallery.EmailAddress;

                try
                {
                    await gClass.SaveAsync();
                    ViewBag.showForm = 0;
                    gallery = getGalleryEntity(gClass);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            return await baseView(gallery);
        }

        public ActionResult AddArtwork()
        {
            return PartialView("~/Views/AddArtwork/Index.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> AddArtwork(ArtworkEntity registerData, HttpPostedFileBase file)
        {

            byte[] data;

            using (Stream inputStream = file.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
            }

            var name = "photo.jpg";
            var parseFile = new Parse.ParseFile(name, data);

            try
            {

                if (parseFile.IsDirty)
                {
                    await parseFile.SaveAsync();
                    Debug.WriteLine("Data to save");

                }
                else
                {
                    Debug.WriteLine("No data to save");
                }

                Debug.WriteLine(parseFile.Url.ToString());


                Debug.WriteLine("IMAGE SAVED SUCCESSFULLY");
            }
            catch
            {
                Debug.WriteLine("IMAGE COULD NOT BE SAVED");
            }

            var fileUrl = parseFile.Url.ToString();

            ViewBag.showForm = 1;
            if (!userIsGalleryOwner())
            {
                return returnFailedUserView();
            }

            GalleryEntity gallery;
            GalleryParseClass galleryClass;
            try
            {
                var query = from item in new ParseQuery<GalleryParseClass>()
                            where item.ObjectId == registerData.ParentGalleryParseID
                            select item;
                galleryClass = await query.FirstAsync();
                gallery = getGalleryEntity(galleryClass);
            }

            catch (Exception ex)
            {
                Debug.WriteLine("There was an error returning owned galleries base view :: " + ex);
                gallery = new GalleryEntity();
                returnFailedUserView();
                galleryClass = new GalleryParseClass();// this is to silence a null error that couldn't happen
            }

            if (ModelState.IsValid)
            {
                ArtworkParseClass artwork = new ArtworkParseClass()
                {
                    Name = registerData.Name,
                    ArtistID = registerData.Artist,
                    Description = registerData.Description,
                    ImageURL = fileUrl,
                    GalleryID = registerData.ParentGalleryParseID,
                    FileOwnerId = getUserId(),
                    Style = registerData.Style, 
                };

                await artwork.SaveAsync();
                if (galleryClass.HasArtwork == 0) { galleryClass.HasArtwork = 1; await galleryClass.SaveAsync(); }

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

                IEnumerable<ArtworkParseClass> artworks = await query.FindAsync();
                foreach (ArtworkParseClass item in artworks)
                {
                    Debug.WriteLine("Processing artwork: " + item.Name + " with artist ID : " + item.ArtistID);
                    G.ArtworkEntities.Add(getArtworkEntity(item));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("There was an error returning owned galleries base view :: " + ex);
                G.ArtworkEntities = new List<ArtworkEntity>();
            }

            var array = new List<string>();

            foreach (ArtworkEntity item in G.ArtworkEntities){
                array.Add(item.Artist);
            }

            try
            {
                var query = from item in new ParseQuery<ArtistParseClass>()
                            where array.Contains(item.ObjectId)
                            select item;

                IEnumerable<ArtistParseClass> artworks = await query.FindAsync();
                foreach (ArtistParseClass item in artworks)
                {
                      G.ArtistEntities.Add(getArtistEntity(item));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("There was an error returning owned galleries base view :: " + ex);
                G.ArtistEntities = new List<ArtistEntity>();
            }

            if(G.ArtworkAdd == null) { G.ArtworkAdd = new ArtworkEntity(); }


            G.ArtworkAdd.addArtworkFormlistItem = new List<SelectListItem>();
            IList<ArtistEntity> artistAddList = new List<ArtistEntity>();

            try
            {
                var query = from item in new ParseQuery<ArtistParseClass>()
                            where item.GalleryOwnerID == getUserId()
                            select item;
                 
                foreach (ArtistParseClass artist in await query.FindAsync())
                {
                    Debug.WriteLine("Processing artist - "+ artist.Name);
                    artistAddList.Add(getArtistEntity(artist));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("There was an error returning owned galleries base view :: " + ex);
                G.ArtistEntities = new List<ArtistEntity>();
            }

            foreach (ArtistEntity item in artistAddList)
            {
                Debug.WriteLine("Adding artist - " + item.Name);

                G.ArtworkAdd.addArtworkFormlistItem.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.ParseID
                });
            }

            try
            {
                var query = from item in new ParseQuery<GeneralParseUserData>()
                            where array.Contains(item.UserId)
                            select item;

                foreach (GeneralParseUserData item in await query.FindAsync())
                {
                    G.ArtistUserEntities.Add(getArtistUserEntity(item));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("There was an error returning owned galleries base view :: " + ex);
                G.ArtistEntities = new List<ArtistEntity>();
            }

            return View("~/Views/Gallery/GalleryView.cshtml", "~/Views/Shared/_LayoutLoggedIn.cshtml", G );
        }

  
    }
}

