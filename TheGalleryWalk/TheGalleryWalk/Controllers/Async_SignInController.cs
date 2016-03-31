using Parse;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TheGalleryWalk.Models;

namespace TheGalleryWalk.Controllers
{
    public class Async_SignInController : BaseValidatorController
    {


        public ActionResult LoginNext()
        {
            Debug.WriteLine("Login Next Called from Async Sign In");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LoginNext(LoginData loginData, FormCollection data)
        {
            ViewBag.showForm = 1;

            if (ModelState.IsValid)
            {
                try
                {
                    await ParseUser.LogInAsync(loginData.EmailAddress, loginData.Password);
                    var user = ParseUser.CurrentUser;
                    logInUser(user);
                    
                    if (userIsGalleryOwner())
                    {
                        return RedirectToAction("OwnedGalleries", "OwnedGalleries", null);
                    }
                    else if(userIsArtist()) {
                        return RedirectToAction("Artist_OwnedArtwork", "Artist_OwnedArtwork", null);
                    }
                  
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("There was an error " + ex);
                    return View("ErrorSignIn");
                }

            }// end if ModelState.IsValid

            Debug.WriteLine("ReachedModel NotValid");

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

     

        [HttpPost]
        public ActionResult Login(string donateForm, LoginDataPrecheck loginData, FormCollection variables)
        {
            if (ModelState.IsValid)
            {
                Debug.WriteLine("The Model is valid!");
                ViewBag.Message = loginData.EmailAddress;
                return View("LoginNext");
            }
            else
            {
                return View("Login");
            }
        }


    }
}