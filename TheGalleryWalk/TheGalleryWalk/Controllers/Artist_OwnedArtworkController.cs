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
using System.IO;

namespace TheGalleryWalk.Controllers
{
    public class Artist_OwnedArtworkController : BaseValidatorController
    {
        // GET: Artist_OwnedArtwork
        public async Task<ActionResult> Artist_OwnedArtwork()
        {
            ViewBag.showForm = 0;
            if (userIsArtist())
            {
                return await returnBaseView(getArtistUserEntity(await getUserData()));
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
        public async Task<ActionResult> AddArtwork(ArtworkEntity registerData, HttpPostedFileBase file)
        {
            if (!userIsArtist())
            {
                return returnFailedUserView();
            }

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
            GeneralParseUserData userData = await getUserData();
            ArtistUserEntity artistUser =  getArtistUserEntity(userData);

            if (ModelState.IsValid)
            {
                var Entity = new ArtworkParseClass()
                {
                    Name = registerData.Name,
                    GalleryID = null,
                    ArtistID = getUserId(),
                    Description = registerData.Description,
                    ImageURL = fileUrl,
                    FileOwnerId = getUserId(),
                    Style = registerData.Style,
                };


                try
                {
                    await Entity.SaveAsync();
                    if (userData.HasArtwork == 0) { userData.HasArtwork = 1; await userData.SaveAsync(); }
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

        public async Task<ActionResult> returnBaseView(ArtistUserEntity artistUser)
        {
            if (artistUser.ArtworkAdd == null) { artistUser.ArtworkAdd = new ArtworkEntity(); }

            try
            {
                var query = from item in new ParseQuery<ArtworkParseClass>()
                            where item.ArtistID == getUserId()
                            select item;

                IEnumerable<ArtworkParseClass> artwork = await query.FindAsync();

                foreach( ArtworkParseClass art in artwork)
                {
                    artistUser.ArtworkEntities.Add(getArtworkEntity(art));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("There was an error returning owned galleries base view :: " + ex);
            }

            return View("~/Views/Artist_OwnedArtwork/OwnedArtwork.cshtml", "_LayoutArtistLoggedIn", artistUser);
        }
    }
}