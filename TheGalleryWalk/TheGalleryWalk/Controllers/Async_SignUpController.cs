using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheGalleryWalk.Models;
using Parse;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;

namespace TheGalleryWalk.Controllers
{
    public class Async_SignUpController : AsyncController
    {

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Signup(GalleryOwnerEntity registerData)
        {
            if (ModelState.IsValid)
            {

                var user = new GeneralParseUser()
                {
                    Username = registerData.EmailAddress,
                    Password = registerData.Password,
                    Email = registerData.EmailAddress,
                    UserType = "GalleryOwnerUser"
                };


                try { await user.SignUpAsync(); }
                catch (Exception ex)
                {
                    Debug.WriteLine("There was an error " + ex);
                    return View("ErrorSignUp");
                }

                var userInfo = new GeneralParseUserData()
                {
                    Name = registerData.Name,
                    PhoneNumber = registerData.PhoneNumber,
                    Enabled = 0,
                    UserId = user.ObjectId,
                    UserType = "GalleryOwnerUser",
                    MyFavoriteArtists = new List<string>(),
                    MyFavoriteGalleries = new List<string>(),
                    IsBanned = 1,
                    HasArtwork = 0,
                    Email = user.Email,
                    AcceptedGalleryFollowers = new List<string>(),
                };

                try
                {
                    await userInfo.SaveAsync();
                    return View("CompletedSignUp");
                }
                catch (Exception ex)
                {
                    await user.DeleteAsync();
                    Debug.WriteLine("There was an error " + ex);
                    return View("ErrorSignUp");
                }
            
               }// end if ModelState.IsValid
            return View();
        }
    }
}