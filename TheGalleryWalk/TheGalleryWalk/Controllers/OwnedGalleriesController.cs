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
    public class OwnedGalleriesController : BaseValidatorController
    {      
        public async Task<ActionResult> OwnedGalleries()
        {
            ViewBag.showForm = 0;

            if (this.verifyUser())
            {
                return await returnBaseView(getGalleryOwnerEntity(await getUserData()));
            }
            else
            {
                return returnFailedUserView();
            }
        }

        public ActionResult AddGallery()
        {
            return PartialView("~/Views/AddGallery/Index.cshtml");
        }


        [HttpPost]
        public async Task<ActionResult> AddGallery(GalleryEntity registerData, HttpPostedFileBase file)
        {
            if (!verifyUser()) { return returnFailedUserView(); }

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

            GalleryOwnerEntity galleryOwner = getGalleryOwnerEntity(await getUserData());

            if (ModelState.IsValid)
            {
                var galleryEntity = new GalleryParseClass()
                {
                    Name = registerData.Name,
                    Email = registerData.EmailAddress,
                    Address = registerData.Address,
                    PhoneNumber = registerData.PhoneNumber,
                    Website = registerData.Website,
                    ImageURL = fileUrl,
                    GalleryOwnerID = getUserId(),
                    FileOwnerId = getUserId(),
                    HasArtwork = 0,
                    IsBanned = 1
                };

                Debug.WriteLine("OWNER ID ON SAVED GALLERY OBJECT :: " + galleryEntity.GalleryOwnerID);

                try
                {
                    await galleryEntity.SaveAsync();
                    ViewBag.showForm = 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            else
            {
                galleryOwner.GalleryAdd = registerData;
            }

            return await returnBaseView(galleryOwner);
        }


        public async Task<ActionResult> returnBaseView(GalleryOwnerEntity G_Owner)
        {
            if (G_Owner.GalleryAdd == null ) { G_Owner.GalleryAdd = new GalleryEntity(); }

            try
            {
                var query = from item in new ParseQuery<GalleryParseClass>()
                            where item.GalleryOwnerID == getUserId()
                            select item;

                foreach (GalleryParseClass gallery in await query.FindAsync())
                {
                    G_Owner.GalleryEntities.Add(getGalleryEntity(gallery));
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("There was an error returning owned galleries base view :: "+ ex);
                G_Owner.GalleryEntities = new List<GalleryEntity>();
            }
               
            return View("~/Views/OwnedGalleries/OwnedGalleries.cshtml", "_LayoutLoggedIn", G_Owner);
       }
    }
}