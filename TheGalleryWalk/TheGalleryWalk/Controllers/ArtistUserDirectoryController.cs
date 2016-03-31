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
    public class ArtistUserDirectoryController : BaseValidatorController
    {
        public async Task<ActionResult> ArtistUserDirectory()
        {
            /* This query will be updated to remove artists without artwork and other things we'll need*/
            var query = from item in new ParseQuery<GeneralParseUserData>()
                        where item.Get<string>("UserType") == "ArtistUser"
                        where item.Get<string>("MyFavoriteGalleries") != null
                        where item.Get<string>("MyFavoriteArtists") != null
                        select item;

           IEnumerable<GeneralParseUserData> artistusers = await query.FindAsync();
                         
          if (userIsGalleryOwner())
           {
              return View("~/Views/ArtistUserDirectory/ArtistUserDirectory.cshtml", "_LayoutLoggedIn", artistusers);
           }
           else if(userIsArtist())
           {
             return View("~/Views/ArtistUserDirectory/ArtistUserDirectory.cshtml", "_LayoutArtistLoggedIn", artistusers);
           }
            else
            {
                return View("~/Views/ArtistUserDirectory/ArtistUserDirectory.cshtml", "_Layout", artistusers);
            }
        }
    }
}