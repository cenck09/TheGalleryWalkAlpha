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
    public class ArtistUserInfoController : AsyncController
    {

        public ActionResult ArtistUserInfoView(ArtistParseUser artistUser)
        {
            return baseView(artistUser);
        }

        public async Task<ActionResult> followArtist(ArtistParseUser artistUser)
        {
            try
            {
                if (this.verifyUser())
                {
                    ParseUser user = ParseUser.CurrentUser;
  
                    Debug.WriteLine("\n\n ----------- Adding artist for user type ---- "+user.Get<string>("UserType")+" \n\n");
                    IList<string> followedByArtistList = user.Get<IList<string>>("MyFavoriteArtists");
                    followedByArtistList.Add(artistUser.ObjectId);
                    user["MyFavoriteArtists"] = followedByArtistList;
                    foreach (var item in followedByArtistList)
                    {
                        Debug.WriteLine("Fav artist id list on follow :"+ item);
                    }
                    await user.SaveAsync();
                    ViewBag.AddedArtist = 1; // for javascript to post a window for success
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to save to artist"+ ex);
                ViewBag.AddedArtist = 2; // for script to post it failed to the user
            }
            return baseView(artistUser);
        }


        public async Task<ActionResult> unfollowArtist(ArtistParseUser artistUser)
        {
            try
            {
                if (this.verifyUser())
                {
                    ParseUser user = ParseUser.CurrentUser;

                    Debug.WriteLine("\n\n ----------- Adding artist for user type ---- " + user.Get<string>("UserType") + " \n\n");
                    IList<string> followedByArtistList = user.Get<IList<string>>("MyFavoriteArtists");
                    followedByArtistList.Remove(artistUser.ObjectId);
                    user["MyFavoriteArtists"] = followedByArtistList;

                    await user.SaveAsync();
                    ViewBag.AddedArtist = 3; // for javascript to post a window for success
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to save to artist" + ex);
                ViewBag.AddedArtist = 4; // for script to post it failed to the user
            }
            return baseView(artistUser);
        }



        public ActionResult baseView(ArtistParseUser artistUser)
        {
            if (this.verifyUser())
            {
                if ("GalleryOwnerUser".Equals(ParseUser.CurrentUser.Get<string>("UserType")))
                {
                    return View("~/Views/ArtistUserInfo/ArtistUserInfoView.cshtml", "_LayoutLoggedIn", artistUser);
                }
                else
                {
                    return View("~/Views/ArtistUserInfo/ArtistUserInfoView.cshtml", "_LayoutArtistLoggedIn", artistUser);
                }
            }
            else
            {
                return View("~/Views/ArtistUserInfo/ArtistUserInfoView.cshtml", "_Layout", artistUser);
            }
        }
        public bool verifyUser()
        {
            var user = ParseUser.CurrentUser;
            if (user == null)
            {
                return false;
            }
            if (user.IsAuthenticated)
            {
                return true;
            }
            else { return false; }
        }
    }
}