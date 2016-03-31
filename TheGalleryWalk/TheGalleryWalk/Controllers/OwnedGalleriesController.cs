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
    public class OwnedGalleriesController : AsyncController
    {      
        public async Task<ActionResult> OwnedGalleries()
        {
            ViewBag.showForm = 0;
            var user = ParseUser.CurrentUser;

            if (this.verifyUser(user))
            {
                GalleryOwnerParseUserData galleryParseUser = new GalleryOwnerParseUserData().getInstanceFromParseObject(user);
                return await returnBaseView(galleryParseUser.toEntityWithSelf());
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
            var userInstance = ParseUser.CurrentUser;

            GalleryOwnerParseUserData user = new GalleryOwnerParseUserData().getInstanceFromParseObject(userInstance);
    
            var galleryOwner = user.toEntityWithSelf();

            if (!verifyUser(userInstance))
            {
                return returnFailedUserView();
            }

            ViewBag.showForm = 1;

            if (ModelState.IsValid)
            {
                var galleryEntity = new GalleryParseClass()
                {
                    Name = registerData.Name,
                    Email = registerData.EmailAddress,
                    Address = registerData.Address,
                    PhoneNumber = registerData.PhoneNumber,
                    Website = registerData.Website,
                    GalleryOwnerID = userInstance.ObjectId,
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
                            where item.GalleryOwnerID == G_Owner.ParseID
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