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
                    artistUser.ArtworkEntities.Add(new ArtworkEntity()
                    {
                        Name = item.Name,
                        ParseID = item.ObjectId,
                        Artist = item.ArtistID,
                    });
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