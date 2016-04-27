using Parse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using TheGalleryWalk.Models;

namespace TheGalleryWalk.Controllers
{
    public class ArtistUserInfoController : BaseValidatorController
    {

        public async Task<ActionResult> ArtistUserInfoView(ArtistUserEntity artistUser)
        {
            Debug.WriteLine("\n\n ---- Artist ID on infowindow first load "+artistUser.ParseID + " - Name - "+ artistUser.Name);
            return await baseView(artistUser); }
      
        public async Task<ActionResult> followArtist(ArtistUserEntity artistUser)
        {
            try
            {
                if (this.verifyUser())
                {
                    GeneralParseUserData userData = await getUserData();
                    Debug.WriteLine("\n\n ----- userData before following artist --"+userData.UserId + " --name-- "+ userData.Name);
                    if(userData.MyFavoriteArtists == null) { Debug.WriteLine("Follow artist found null myFavoriteArtists"); userData.MyFavoriteArtists = new List<string>(); }

                    IList<string> favArtist = userData.MyFavoriteArtists;
                    favArtist.Add(artistUser.ParseID);
                    userData.MyFavoriteArtists = favArtist;
                    await userData.SaveAsync();

                    ViewBag.AddedArtist = 1; // for javascript to post a window for success
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to save to artist"+ ex);
                ViewBag.AddedArtist = 2; // for script to post it failed to the user
            }
            return await baseView(artistUser);
        }


        public async Task<ActionResult> unfollowArtist(ArtistUserEntity artistUser)
        {
            try
            {
                if (this.verifyUser())
                {
                    GeneralParseUserData userData = await getUserData();
                    Debug.WriteLine("Fav artist list before remove" + userData.MyFavoriteArtists.Count);
                    IList<string> favArtist = userData.MyFavoriteArtists;
                    favArtist.Remove(artistUser.ParseID);

                    userData.MyFavoriteArtists = favArtist;
                    await userData.SaveAsync();

                    Debug.WriteLine("Fav artist list after remove"+ userData.MyFavoriteArtists.Count);

                    ViewBag.AddedArtist = 3; // for javascript to post a window for success
                }
                else
                {
                    Debug.WriteLine("Failed to verify current user");
                    return returnFailedUserView();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to save to artist" + ex);
                ViewBag.AddedArtist = 4; // for script to post it failed to the user
            }
            return await baseView(artistUser);
        }

        [HttpPost]
        public async Task<ActionResult> addArtworkToGallery(ArtworkEntity artwork)
        {
            Debug.WriteLine("\n\n -- Clicked add artwork to gallery");
            ArtworkParseClass art = await getArtworkParseObjectFromEntity(artwork);
            art.GalleryID = artwork.ParentGalleryParseID;
            await art.SaveAsync();
            return await baseView(await getArtistUserForArtwork(artwork));
        }

        public async Task<ActionResult> removeArtworkFromGallery(ArtworkEntity artwork)
        {
            Debug.WriteLine("\n\n -- Clicked remove artwork to gallery");
            ArtworkParseClass art = await getArtworkParseObjectFromEntity(artwork);
            art.GalleryID = null;
            await art.SaveAsync();
            return await baseView(await getArtistUserForArtwork(artwork));
        }

        public async Task<ActionResult> baseView(ArtistUserEntity artistUser)
        {
            IEnumerable<ArtworkParseClass> artwork = new List<ArtworkParseClass>();
            try
            {
                var query = from item in new ParseQuery<ArtworkParseClass>()
                            where item.ArtistID == artistUser.ParseID
                            select item;
                Debug.WriteLine("\n\n Parse Artist User ID"+artistUser.ParseID);

                artwork = await query.FindAsync();
             
            }
            catch (Exception ex)
            {
                Debug.WriteLine("There was an error returning base view :: " + ex);
            }

            foreach (ArtworkParseClass item in artwork)
            {
                try
                {
                    artistUser.ArtworkEntities.Add(getArtworkEntity(item));
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Error found while artwork entity" + ex);
                }

            }
            ViewBag.IsFollowing = 0;
            if (verifyUser())
            {
                GeneralParseUserData userData = await getUserData();
                if (userData != null)
                {
                    foreach(string favs in userData.MyFavoriteArtists)
                    {
                        Debug.WriteLine(" Followed Artist array before base view -- "+favs);
                    }
                    if (userData.MyFavoriteArtists.Contains(artistUser.ParseID))
                    {
                        ViewBag.IsFollowing = 1;
                    }
                    else
                    {
                        ViewBag.IsFollowing = 2;
                    }
                }
            }

            if (userIsGalleryOwner())
            {
                List<SelectListItem> list = new List<SelectListItem>();
                var query = from item in new ParseQuery<GalleryParseClass>()
                            where item.GalleryOwnerID == getUserId()
                            select item;

                foreach (GalleryParseClass item in await query.FindAsync())
                {
                    Debug.WriteLine("Adding Gallery - " + item.Name);
                    list.Add(new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.ObjectId
                    });
                }
                foreach (ArtworkEntity art in artistUser.ArtworkEntities)
                {
                    if (string.IsNullOrEmpty(art.ParentGalleryParseID))
                    {
                        art.GalleryListForArtworkSharing = list;
                        art.OwnershipState = "Unowned";
                    }
                    else
                    {
                        art.OwnershipState = "Owned";
                    }
                }

                return View("~/Views/ArtistUserInfo/ArtistUserInfoView.cshtml", "_LayoutLoggedIn", artistUser);
            }
            else if(userIsArtist())
            {
                return View("~/Views/ArtistUserInfo/ArtistUserInfoView.cshtml", "_LayoutArtistLoggedIn", artistUser);
            }
            else
            {
                return View("~/Views/ArtistUserInfo/ArtistUserInfoView.cshtml", "_Layout", artistUser);
            }
        }

        private async Task<ArtistUserEntity> getArtistUserForArtwork(ArtworkEntity artwork)
        {
            var query = from item in new ParseQuery<GeneralParseUserData>()
                        where item.UserId == artwork.Artist
                        select item;
            try { return (getArtistUserEntity(await query.FirstAsync())); }
            catch { return new ArtistUserEntity(); }
        }

        private async Task<ArtworkParseClass> getArtworkParseObjectFromEntity(ArtworkEntity artwork)
        {
            var query = from item in new ParseQuery<ArtworkParseClass>()
                        where item.ObjectId == artwork.ParseID
                        select item;
            try { return await query.FirstAsync(); }
            catch (Exception ex) { Debug.WriteLine(ex); 
                  return default(ArtworkParseClass);
            }
        }

    }
}