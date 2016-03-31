using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public bool verifyUser()
        {
            if (Session["UserId"] == null) {  return false; }
            else { return true; }
        }

        public void logInUser( ParseUser user)
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