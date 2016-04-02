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
    public class BaseValidatorController : AsyncController
    {

        private void setUserId(string userId) { Session["UserId"] = userId;}
        public string getUserId() { return Session["UserId"].ToString(); }

        private void setUserType(string userType) { Session["UserType"] = userType;  }
        public string getUserType() {return Session["UserType"].ToString(); }

        public bool userIsArtist() { return ( verifyUser() && "ArtistUser".Equals(getUserType())); }
        public bool userIsGalleryOwner() { return (verifyUser() && "GalleryOwnerUser".Equals(getUserType())); }



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
                GalleryEntities = new List<GalleryParseClass>(),
                ArtistEntities = new List<ArtistParseClass>(),
            };
        }

        public ArtistUserEntity getArtistUserEntity(GeneralParseUserData userData)
        {
            return new ArtistUserEntity()
            {
                ParseID = userData.UserId.ToString(),
                Name = userData.Name,
                Enabled = userData.Enabled,
                PhoneNumber= userData.PhoneNumber,
                ArtworkAdd = new ArtworkEntity(),
                ArtworkEntities = new List<ArtworkEntity>(), 
            };
        }
        public ArtworkEntity getArtworkEntity(ArtworkParseClass artwork)
        {
            return new ArtworkEntity()
            {
                parseID = artwork.ObjectId,
                Name = artwork.Name,
                ParentGalleryParseID = artwork.GalleryID,
                Artist = artwork.ArtistID,
                GalleryListForArtworkSharing = new List<SelectListItem>(),
                Description = artwork.Description,
                OwnershipState = "Unowned",
                Style = artwork.Style,
            };
        }
        public GalleryEntity getGalleryEntity(GalleryParseClass gallery)
        {
            return new GalleryEntity()
            {
                Name = gallery.Name,
                ArtworkEntities = new List<ArtworkParseClass>(),
                Address = gallery.Address,
                PhoneNumber = gallery.PhoneNumber,
                ArtistAdd = new ArtistEntity(),
                ArtistEntities = new List<ArtistParseClass>(),
                ArtworkAdd = new ArtworkEntity(),
                EmailAddress = gallery.Email,
                ParseID = gallery.ObjectId,
                GalleryOwnerID = gallery.GalleryOwnerID,
                Website = gallery.Website,
            };
        }

        public ArtistEntity getArtistEntity(ArtistParseClass artist)
        {
            return new ArtistEntity()
            {
                parseID = artist.ObjectId,
                Name = artist.Name,
                ParentGalleryParseID = artist.GalleryID,
                Death = artist.Death,
                Birth = artist.Birth,
                ArtworkEntities = new List<ArtworkEntity>(),
                Description = artist.Description,
                Style = artist.Style,
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