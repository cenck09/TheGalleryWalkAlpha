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
    public class Artist_Async_SignUpController : Controller
    {
     
        public ActionResult Signup()
        {
            return View("~/Views/Artist_Async_SignUp/Signup.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> Signup(ArtistUserEntity registerData)
        {
            if (ModelState.IsValid)
            {
                var user = new ArtistParseUser()
                {
                    Username = registerData.EmailAddress,
                    Password = registerData.Password,
                    Email = registerData.EmailAddress,
                    Name = registerData.Name,
                    PhoneNumber = registerData.PhoneNumber,
                    Enabled = 0
                };

                Debug.WriteLine("Creating user: " + user.Name);

                try
                {
                    await user.SignUpAsync();
                    return View("CompletedSignUp");

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("There was an error " + ex);
                    return View("ErrorSignUp");
                }

            }// end if ModelState.IsValid

            return View();
        }
    }
}