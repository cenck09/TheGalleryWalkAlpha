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
    public class ArtworkController : AsyncController
    {

        public async Task<ActionResult> ArtworkView(ArtworkEntity selectedArtwork)
        {
             ViewBag.showForm = 0;

            var user = ParseUser.CurrentUser;
            if(!verifyUser(user))
            {
                return returnFailedUserView();
            }
            else
            {
                return await baseView(user, selectedArtwork);
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

                ArtworkParseClass gClass = await query.FirstAsync();

                gClass.Name = artwork.Name;
                gClass.Style = artwork.Style;
                gClass.Description = artwork.Description;

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

            return await baseView(ParseUser.CurrentUser, artwork);
        }



        public ArtworkEntity getArtworkEntityForParseObject(ParseObject Artwork)
        {
            ArtworkEntity artworkToReturn = new ArtworkEntity();
            artworkToReturn.Name = Artwork.Get<string>("Name");
            artworkToReturn.Artist = Artwork.Get<string>("Artist");
            artworkToReturn.Description = Artwork.Get<string>("Description");
            

            return artworkToReturn;
        }


        public async Task<ActionResult> baseView(ParseUser user, ArtworkEntity selectedArtwork)
        {
            var artworkQuery = ParseObject.GetQuery("Artwork");
            var userArtworkQuery = ParseObject.GetQuery("Artwork");
            artworkQuery = artworkQuery.WhereEqualTo("objectId", selectedArtwork.parseID);


            IList<string> userArtworkIds; // = user.Get<IList<string>>("Galleries");
            try
            {
                userArtworkIds = user.Get<IList<string>>("Artwork");
            }
            catch (Exception ex)
            {
                userArtworkIds = new List<string>();
                Debug.WriteLine("Failed to get artwork list from user "+ex);
            }
            userArtworkQuery = userArtworkQuery.WhereContainedIn("objectId", userArtworkIds);

            IEnumerable<ParseObject> userArtworkEntities;
            try
            {
                userArtworkEntities = await userArtworkQuery.FindAsync();
            }catch(Exception ex)
            {
                userArtworkEntities = new List<ParseObject>();
                Debug.WriteLine("Failed to get artwork entities : "+ex);
            }

            
           

            ParseObject Gallery = null;
            try
            {
                Gallery = await artworkQuery.FirstAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            IList<string> artworkIds; // = user.Get<IList<string>>("Galleries");
            try
            {
                artworkIds = Gallery.Get<IList<string>>("Artwork");
            }
            catch(Exception ex)
            {
                artworkIds = new List<string>();
                Debug.WriteLine(ex);
            }
            //var artworkQuery = ParseObject.GetQuery("Artwork");


            // IList<string> artworkIds; // = user.Get<IList<string>>("Galleries");
            try
            {
                artworkIds = Gallery.Get<IList<string>>("Artwork");
            }
            catch (Exception ex)
            {
                artworkIds = new List<string>();
                Debug.WriteLine(ex);
            }

          
            
            return View("~/Views/Artwork/ArtworkView.cshtml", "~/Views/Shared/_LayoutLoggedIn.cshtml", selectedArtwork);
        }


        public ActionResult returnFailedUserView()
        {
            return View("../Home/Index", "_Layout");
        }

        private bool verifyUser(ParseUser user)
        {
            if (user == null)
            {
                return false;

            }
            else if (!user.IsAuthenticated)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}