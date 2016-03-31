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
            return await baseView(artistUser);
        }

        public async Task<ActionResult> followArtist(ArtistUserEntity artistUser)
        {
            try
            {
                if (this.verifyUser())
                {
                    GeneralParseUserData userData = await getUserData();
                    userData.MyFavoriteArtists.Add(artistUser.ParseID);
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
                    userData.MyFavoriteArtists.Remove(artistUser.ParseID);
                    await userData.SaveAsync();
                    ViewBag.AddedArtist = 3; // for javascript to post a window for success
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to save to artist" + ex);
                ViewBag.AddedArtist = 4; // for script to post it failed to the user
            }
            return await baseView(artistUser);
        }



        public async Task<ActionResult> baseView(ArtistUserEntity artistUser)
        {
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

            if (userIsGalleryOwner())
            {
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
    }
}