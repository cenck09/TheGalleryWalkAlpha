using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Parse;
using TheGalleryWalk.Models;
using System.Diagnostics;

namespace TheGalleryWalk.Controllers
{
    public class ArtistController : BaseValidatorController
    {
        public ActionResult updateGalleryInfo()
        {
            return PartialView("~/Views/EditArtist/EditArtistPartialView.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> updateArtistInfo(ArtistEntity artist)
        {
            if (!userIsGalleryOwner())
            {
                return returnFailedUserView();
            }

            ViewBag.showForm = 2;
            if (ModelState.IsValid)
            {
                var query = from item in new ParseQuery<ArtistParseClass>()
                            where item.ObjectId == artist.parseID
                            select item;

                ArtistParseClass gClass = await query.FirstAsync();
                if (gClass.FileOwnerId != getUserId())
                {
                   return returnFailedUserView();
                }

                gClass.Name = artist.Name;
                gClass.Style = artist.Style;
                gClass.Description = artist.Description;

                try
                {
                    await gClass.SaveAsync();
                    ViewBag.showForm = 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            return await baseView(artist);
        }

        public async Task<ActionResult> ArtistView(ArtistEntity selectedArtist)
        {
           ViewBag.showForm = 0;
  
            if(!userIsGalleryOwner()) { return returnFailedUserView();  }
            else { return await baseView(selectedArtist);  }

        }// EOM
     
    
        public async Task<ActionResult> baseView(ArtistEntity artistUser)
        {
            try
            {
                var query = from item in new ParseQuery<ArtworkParseClass>()
                            where item.ArtistID == artistUser.parseID
                            select item;

               IEnumerable<ArtworkParseClass> artworkParseClass = await query.FindAsync();
                foreach(ArtworkParseClass item in artworkParseClass)
                {
                    artistUser.ArtworkEntities.Add(getArtworkEntity(item));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("There was an error returning owned galleries base view :: " + ex);
            }

            if (userIsGalleryOwner())
            {
                return View("~/Views/Artist/ArtistView.cshtml", "~/Views/Shared/_LayoutLoggedIn.cshtml", artistUser);
            }
            else
            {
                return returnFailedUserView();
            }
        }
    }
}