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
    public class GalleryDirectoryController : BaseValidatorController
    {
        public async Task<ActionResult> GalleryDirectory()
        {
            var query = from item in new ParseQuery<GalleryParseClass>()
                        where item.HasArtwork == 1
                        orderby item.Name descending
                        select item;

            IList<GalleryEntity> galleries = new List<GalleryEntity>();

            foreach (GalleryParseClass g in await query.FindAsync())
            {
                galleries.Add(getGalleryEntity(g));
            }

           if (userIsGalleryOwner())
           {
              return View("~/Views/GalleryDirectory/GalleryDirectory.cshtml", "_LayoutLoggedIn", galleries);
           }
           else if(userIsArtist())
           {
             return View("~/Views/GalleryDirectory/GalleryDirectory.cshtml", "_LayoutArtistLoggedIn", galleries);
           }
           else
           {
             return View("~/Views/GalleryDirectory/GalleryDirectory.cshtml", "_Layout", galleries);
           }
        }
    }
}