using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Parse;
using TheGalleryWalk.Models;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TheGalleryWalk.Controllers
{
    public class ArtistUserDirectoryController : AsyncController
    {
        // GET: ArtistUserDirectory
        public async Task<ActionResult> ArtistUserDirectory()
        {
            /* This query will be updated to remove artists without artwork and other things we'll need*/
            var query = from item in new ParseQuery<ParseUser>()
                        where item.Get<string>("UserType") == "ArtistUser"
                        where item.Get<string>("MyFavoriteGalleries") != null
                        where item.Get<string>("MyFavoriteArtists") != null
                        select item;

            IEnumerable<ParseUser> users = await query.FindAsync();

            IList<ArtistParseUser> artistusers = new List<ArtistParseUser>();

            foreach(var item in users)
            {
                Debug.WriteLine("Artist user with id -- "+ item.ObjectId);
                artistusers.Add(new ArtistParseUser().getInstanceFromParseObject(item));
            }

            Debug.WriteLine("\n\n ----- Number of artist users -------  " + artistusers.Count()+"\n\n");

            if (this.verifyUser())
            {              
                if ("GalleryOwnerUser".Equals(ParseUser.CurrentUser.Get<string>("UserType")))
                {
                    return View("~/Views/ArtistUserDirectory/ArtistUserDirectory.cshtml", "_LayoutLoggedIn", artistusers);
                }
                else
                {
                    return View("~/Views/ArtistUserDirectory/ArtistUserDirectory.cshtml", "_LayoutArtistLoggedIn", artistusers);
                }
            }
            else
            {
                return View("~/Views/ArtistUserDirectory/ArtistUserDirectory.cshtml", "_Layout", artistusers);
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