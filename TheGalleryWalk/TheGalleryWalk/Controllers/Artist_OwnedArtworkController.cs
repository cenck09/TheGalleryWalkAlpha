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
    public class Artist_OwnedArtworkController : BaseValidatorController
    {
        // GET: Artist_OwnedArtwork
        public async Task<ActionResult> Artist_OwnedArtwork()
        {
            ViewBag.showForm = 0;
            if (userIsArtist())
            {
                return await returnBaseView(await getArtistUserEntity());
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
            if (!userIsArtist())
            {
                return returnFailedUserView();
            }

            ViewBag.showForm = 1;
            ArtistUserEntity artistUser = await getArtistUserEntity();

            if (ModelState.IsValid)
            {
                var Entity = new ArtworkParseClass()
                {
                    Name = registerData.Name,
                    GalleryID = null,
                    ArtistID = getUserId(),
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

        public async Task<ArtistUserEntity> getArtistUserEntity()
        {
            try
            {
                GeneralParseUserData userData = await getUserData();
                return new ArtistUserEntity()
                {
                    ParseID = userData.UserId,
                    Name = userData.Name,
                    PhoneNumber = userData.PhoneNumber,
                };
            }
            catch (Exception ex) {return new ArtistUserEntity();}
        }

        public async Task<ActionResult> returnBaseView(ArtistUserEntity artistUser)
        {
            if (artistUser.ArtworkAdd == null) { artistUser.ArtworkAdd = new ArtworkEntity(); }

            try
            {
                var query = from item in new ParseQuery<ArtworkParseClass>()
                            where item.ArtistID == artistUser.ParseID
                            select item;

                artistUser.ArtworkEntities = await query.FindAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("There was an error returning owned galleries base view :: " + ex);
                artistUser.ArtworkEntities = new List<ArtworkParseClass>();
            }


            return View("~/Views/Artist_OwnedArtwork/OwnedArtwork.cshtml", "_LayoutArtistLoggedIn", artistUser);
        }

        public ActionResult returnFailedUserView()
        {
            return View("../Home/Index", "_Layout");
        }

    }
}