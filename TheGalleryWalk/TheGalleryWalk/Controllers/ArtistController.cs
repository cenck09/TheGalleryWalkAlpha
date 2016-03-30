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
    public class ArtistController : AsyncController
    {


        public ActionResult updateGalleryInfo()
        {
            return PartialView("~/Views/EditArtist/EditArtistPartialView.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> updateArtistInfo(ArtistEntity artist)
        {
            if (!verifyUser(ParseUser.CurrentUser))
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

            return await baseView(ParseUser.CurrentUser, artist);
        }

        public async Task<ActionResult> ArtistView(ArtistEntity selectedArtist)
        {
             ViewBag.showForm = 0;

            var user = ParseUser.CurrentUser;
            if(!verifyUser(user))
            {
                return returnFailedUserView();
            }
            else
            {
                return await baseView(user, selectedArtist);
            }
           
        }// EOM
     
    
        


        public ArtistEntity getArtistEntityForParseObject(ParseObject Artist)
        {
            ArtistEntity artistToReturn = new ArtistEntity();
            artistToReturn.Name = Artist.Get<string>("Name");
            artistToReturn.Style = Artist.Get<string>("Style");
            artistToReturn.Description = Artist.Get<string>("Description");
            

            return artistToReturn;
        }


        public async Task<ActionResult> baseView(ParseUser user, ArtistEntity selectedArtist)
        {
            var artistQuery = ParseObject.GetQuery("Artist");
            var userArtistQuery = ParseObject.GetQuery("Artist");
            artistQuery = artistQuery.WhereEqualTo("objectId", selectedArtist.parseID);


            IList<string> userArtworkIds; // = user.Get<IList<string>>("Galleries");
            try
            {
                userArtworkIds = user.Get<IList<string>>("Artists");
            }
            catch (Exception ex)
            {
                userArtworkIds = new List<string>();
                Debug.WriteLine("Failed to get artist list from user "+ex);
            }
            userArtistQuery = userArtistQuery.WhereContainedIn("objectId", userArtworkIds);

            IEnumerable<ParseObject> userArtistEntities;
            try
            {
                userArtistEntities = await userArtistQuery.FindAsync();
            }catch(Exception ex)
            {
                userArtistEntities = new List<ParseObject>();
                Debug.WriteLine("Failed to get artist entities : "+ex);
            }


            


            try
            {
                var query = from item in new ParseQuery<ArtworkParseClass>()
                            where item.ArtistID == selectedArtist.parseID
                            select item;

                IEnumerable<ArtworkParseClass> ArtworkEntities = await query.FindAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("There was an error returning owned galleries base view :: " + ex);
                selectedArtist.ArtworkEntities = new List<ArtworkParseClass>();
            }



            ParseObject Gallery = null;
            try
            {
                Gallery = await artistQuery.FirstAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            IList<string> artworkIds; // = user.Get<IList<string>>("Galleries");
            try
            {
                artworkIds = Gallery.Get<IList<string>>("Artworks");
            }
            catch(Exception ex)
            {
                artworkIds = new List<string>();
                Debug.WriteLine(ex);
            }
            var artworkQuery = ParseObject.GetQuery("Artwork");


            //IList<string> artistIds; // = user.Get<IList<string>>("Galleries");
            //try
            //{
            //    artistIds = Gallery.Get<IList<string>>("Artists");
            //}
            //catch (Exception ex)
            //{
            //    artistIds = new List<string>();
            //    Debug.WriteLine(ex);
            //}

          
            
            return View("~/Views/Artist/ArtistView.cshtml", "~/Views/Shared/_LayoutLoggedIn.cshtml", selectedArtist);
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