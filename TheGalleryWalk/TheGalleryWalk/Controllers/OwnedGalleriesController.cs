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
        public async Task<ActionResult> AddGallery(GalleryEntity registerData)
        {
            if (!verifyUser()) { return returnFailedUserView(); }
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
                    GalleryOwnerID = getUserId(),
                    FileOwnerId = getUserId(),
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

                G_Owner.GalleryEntities = await query.FindAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine("There was an error returning owned galleries base view :: "+ ex);
                G_Owner.GalleryEntities = new List<GalleryParseClass>();
            }
               
            return View("~/Views/OwnedGalleries/OwnedGalleries.cshtml", "_LayoutLoggedIn", G_Owner);
       }
    }
}