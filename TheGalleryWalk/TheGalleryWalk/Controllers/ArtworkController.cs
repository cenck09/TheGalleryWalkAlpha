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
    public class ArtworkController : BaseValidatorController
    {

        public async Task<ActionResult> ArtworkView(ArtworkEntity selectedArtwork)
        {
             ViewBag.showForm = 0;

            if(!verifyUser())
            {
                return returnFailedUserView();
            }
            else
            {
                return await baseView(selectedArtwork);
            }
           
        }// EOM



        public ActionResult updateArtworkInfo()
        {
            return PartialView("~/Views/EditArtwork/EditArtworkPartialView.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> updateArtworkInfo(ArtworkEntity artwork)
        {
            ViewBag.showForm = 2;
            if (ModelState.IsValid)
            {
              

                var query = from item in new ParseQuery<ArtworkParseClass>()
                            where item.ObjectId == artwork.parseID
                            select item;


                try
                {
                    Debug.WriteLine("About to get Artwork object from server");
                    ArtworkParseClass gClass = await query.FirstAsync();
                   
                    if(gClass.FileOwnerId != getUserId())
                    {  // the current user does not own this 
                        return returnFailedUserView();
                    }

                    gClass.Name = artwork.Name;
                    gClass.Style = artwork.Style;
                    gClass.Description = artwork.Description;
                    Debug.WriteLine("About to save Artwork object to server");
                    await gClass.SaveAsync();
                    Debug.WriteLine("post Save");
                    ViewBag.showForm = 0;

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(" \n\n\n ---------- There was an error saving the artwork" + ex);
                }
            }
            else
            {
                Debug.WriteLine(" \n\n  ----- The State Is not valid for adding artwork! ----  \n\n");
            }

            return await baseView(artwork);
        }


        public async Task<ActionResult> baseView(ArtworkEntity selectedArtwork)
        {
            var query = from item in new ParseQuery<ArtworkParseClass>()
                        where item.ObjectId == selectedArtwork.parseID
                        select item;
            ArtworkParseClass artwork = await query.FirstAsync();
            if (artwork.FileOwnerId == getUserId())
            {
                ViewBag.showEditOptions = 1;
            }
            else
            {
                ViewBag.showEditOptions = 0;
            }
            return View("~/Views/Artwork/ArtworkView.cshtml", "~/Views/Shared/_LayoutLoggedIn.cshtml", selectedArtwork);
        }      
    }
}