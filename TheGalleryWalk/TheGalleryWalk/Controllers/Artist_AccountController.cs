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
    public class Artist_AccountController : BaseValidatorController
    {
      
        public async Task<ActionResult> artistAccountView()
        {
            ViewBag.showForm = 0;

            if (this.verifyUser())
            {
                return await returnBaseView(getArtistUserEntity(await getUserData()));
            }
            else
            {
                return returnFailedUserView();
            }
        }


        public async Task<ActionResult> toggleArtworkSharing(GalleryOwnerEntity galleryOwner)
        {
            try
            {
                if (this.verifyUser())
                {
                    GeneralParseUserData userData = await getUserData();
                    if (userData.AcceptedGalleryFollowers == null)
                    {
                        Debug.WriteLine("Accepted Gallery is null, setting to new list");
                        userData.AcceptedGalleryFollowers = new List<string>();
                    }

                    if (userData.AcceptedGalleryFollowers.Contains(galleryOwner.ParseID))
                    {
                        IList<string> galleriesList = userData.AcceptedGalleryFollowers;
                        galleriesList.Remove(galleryOwner.ParseID);
                        userData.AcceptedGalleryFollowers = galleriesList;
                        Debug.WriteLine("Removing gallery owner");
                    }
                    else
                    {
                        IList<string> galleriesList = userData.AcceptedGalleryFollowers;
                        galleriesList.Add(galleryOwner.ParseID);
                        userData.AcceptedGalleryFollowers = galleriesList;
                        Debug.WriteLine("Adding gallery owner");
                    }

                    await userData.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to save" + ex);
            }
            ViewBag.ScrollToId = galleryOwner.EmailAddress;
            return await returnBaseView(getArtistUserEntity(await getUserData()));
        }



        public async Task<ActionResult> returnBaseView(ArtistUserEntity user)
        {
            user.OwnersFollowingThisArtistUser = await getGalleryOwnersFollowingArtistUser(user);
            return View("~/Views/Artist_Account/ArtistAccountView.cshtml", "_LayoutArtistLoggedIn", user);
        }


        private async Task<IList<GalleryOwnerEntity>> getGalleryOwnersFollowingArtistUser(ArtistUserEntity artist)
        {
            IList<GalleryOwnerEntity> ownerEntities = new List<GalleryOwnerEntity>();
            try
            {
                var query = from item in new ParseQuery<GeneralParseUserData>()
                            where  item.MyFavoriteArtists.Contains(getUserId())
                            select item;

                foreach (GeneralParseUserData owner in await query.FindAsync())
                {
                    var query2 = from item in new ParseQuery<GalleryParseClass>()
                                 where item.GalleryOwnerID == owner.UserId
                                select item;

                    GalleryOwnerEntity ownerEntity = getGalleryOwnerEntity(owner);
                    foreach (GalleryParseClass gallery in await query2.FindAsync())
                    {
                        ownerEntity.GalleryEntities.Add(getGalleryEntity(gallery));
                    }
                    ownerEntities.Add(ownerEntity);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("There was an error :: " + ex);
            }
            return ownerEntities;
        }

    }
}