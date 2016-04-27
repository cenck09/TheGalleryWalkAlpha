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
                        where item.UserType == "ArtistUser"
                        where item.MyFavoriteGalleries != null
                        where item.MyFavoriteArtists != null
                        where item.Enabled == 1
                        where item.IsBanned == 0
                        where item.HasArtwork == 1
                        where item.AcceptedGalleryFollowers != null
                        select item;

            IList<ArtistUserEntity> artists = new List<ArtistUserEntity>();

            foreach (GeneralParseUserData artistuser in await query.FindAsync())
            {
                artists.Add(getArtistUserEntity(artistuser));
            }

           if (userIsGalleryOwner())
           {
              return View("~/Views/ArtistUserDirectory/ArtistUserDirectory.cshtml", "_LayoutLoggedIn", artists);
           }
           else if(userIsArtist())
           {
             return View("~/Views/ArtistUserDirectory/ArtistUserDirectory.cshtml", "_LayoutArtistLoggedIn", artists);
           }
           else
           {
             return View("~/Views/ArtistUserDirectory/ArtistUserDirectory.cshtml", "_Layout", artists);
           }
        }
    }
}