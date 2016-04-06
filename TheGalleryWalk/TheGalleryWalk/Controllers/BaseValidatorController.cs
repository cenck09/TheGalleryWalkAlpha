using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Parse;
using TheGalleryWalk.Models;
using System.Threading.Tasks;

namespace TheGalleryWalk.Controllers
{
    public class BaseValidatorController : AsyncController
    {

        private void setUserId(string userId) { Session["UserId"] = userId;}
        public string getUserId() { return Session["UserId"].ToString(); }
        private void setUserType(string userType) { Session["UserType"] = userType;  }
        public string getUserType() {return Session["UserType"].ToString(); }


        public bool userIsArtist() { return ( verifyUser() && "ArtistUser".Equals(getUserType())); }
        public bool userIsGalleryOwner() { return (verifyUser() && "GalleryOwnerUser".Equals(getUserType())); }
        public bool userIsFileOwnerOfParseObject(ParseObject entity)
        {
            if (verifyUser())
            {
                if (getUserId() == (string.IsNullOrEmpty(entity.Get<string>("FileOwnerId")) ? "" : entity.Get<string>("FileOwnerId")))
                {
                    return true;
                }
            }
            return false;
        }


        public GalleryOwnerEntity getGalleryOwnerEntity(GeneralParseUserData userData)
        {
            return new GalleryOwnerEntity()
            {
                ParseID = userData.ObjectId,
                Name = userData.Name,
                ArtistAdd = new ArtistEntity(),
                Enabled = userData.Enabled,
                PhoneNumber = userData.PhoneNumber,
                GalleryAdd = new GalleryEntity(),
                MyFavoriteArtists = new List<ArtistUserEntity>(),
                MyFavoriteGalleries = new List<GalleryEntity>(),
                GalleryEntities = new List<GalleryEntity>(),
                ArtistEntities = new List<ArtistEntity>(),
            };
        }
        public ArtistUserEntity getArtistUserEntity(GeneralParseUserData userData)
        {
            return new ArtistUserEntity()
            {
                ParseID = userData.UserId.ToString(),
                Name = string.IsNullOrEmpty(userData.Name) ? "" : userData.Name,
                Enabled = userData.Enabled,
                PhoneNumber = string.IsNullOrEmpty(userData.PhoneNumber) ? "" : userData.PhoneNumber,
                ArtworkAdd = new ArtworkEntity(),
                ArtworkEntities = new List<ArtworkEntity>(), 
                PermittedGalleries =  userData.AcceptedGalleryFollowers,
                EmailAddress = userData.Email,
                OwnersFollowingThisArtistUser = new List<GalleryOwnerEntity>(),
            };
        }
        public ArtworkEntity getArtworkEntity(ArtworkParseClass artwork)
        {
            return new ArtworkEntity()
            {
                ParseID = artwork.ObjectId,
                Name = artwork.Name,
                ParentGalleryParseID = string.IsNullOrEmpty(artwork.GalleryID) ? "" : artwork.GalleryID,
                Artist = artwork.ArtistID,
                ImageURL = string.IsNullOrEmpty(artwork.ImageURL) ? "https://dl.dropboxusercontent.com/u/13628780/ArtworkPlaceholder.jpg" : artwork.ImageURL,
                GalleryListForArtworkSharing = new List<SelectListItem>(),
                Description = string.IsNullOrEmpty(artwork.Description) ? "" : artwork.Description,
                OwnershipState = "Unowned",
                Style = string.IsNullOrEmpty(artwork.Style) ? "" : artwork.Style,
                ShouldAddSharingOptions = false,
            };
        }
        public GalleryEntity getGalleryEntity(GalleryParseClass gallery)
        {
            return new GalleryEntity()
            {
                Name = gallery.Name,
                ArtworkEntities = new List<ArtworkEntity>(),
                Address = string.IsNullOrEmpty(gallery.Address) ? "" : gallery.Address,
                PhoneNumber = string.IsNullOrEmpty(gallery.PhoneNumber) ? "" : gallery.PhoneNumber,
                ArtistAdd = new ArtistEntity(),
                ArtistEntities = new List<ArtistEntity>(),
                ArtworkAdd = new ArtworkEntity(),
                EmailAddress = string.IsNullOrEmpty(gallery.Email) ? "" : gallery.Email,
                ParseID = gallery.ObjectId,
                GalleryOwnerID = gallery.GalleryOwnerID,
                Website = string.IsNullOrEmpty(gallery.Website) ? "" : gallery.Website,
                ArtistUserEntities = new List<ArtistUserEntity>()
            };
        }
        public ArtistEntity getArtistEntity(ArtistParseClass artist)
        {
            return new ArtistEntity()
            {
                ParseID = artist.ObjectId,
                Name = artist.Name,
                ParentGalleryParseID = artist.GalleryID,
                Death = string.IsNullOrEmpty(artist.Death) ? "" : artist.Death,
                Birth = string.IsNullOrEmpty(artist.Birth) ? "" : artist.Birth,
                ImageURL = string.IsNullOrEmpty(artist.ImageURL) ? "https://dl.dropboxusercontent.com/u/13628780/ArtworkPlaceholder.jpg" : artist.ImageURL,
                ArtworkEntities = new List<ArtworkEntity>(),
                Description = string.IsNullOrEmpty(artist.Description) ? "" : artist.Description,
                Style = string.IsNullOrEmpty(artist.Style) ? "" : artist.Style,
                
            };
        }


        public bool verifyUser()
        {
            if (Session["UserId"] == null) {  return false; }
            else { return true; }
        }
        public void logInUser(ParseUser user)
        {
            setUserId(user.ObjectId);
            setUserType(user.Get<string>("UserType"));
     }
        public void logOutUser()
        {
            Session.Clear();
        }
        public async Task<GeneralParseUserData> getUserData()
        {
            var query = from item in new ParseQuery<GeneralParseUserData>()
                        where item.UserId == getUserId()
                        select item;

            return await query.FirstAsync();
        }


        public ActionResult returnFailedUserView()
        {
            return View("../Home/Index", "_Layout");
        }

    }
}