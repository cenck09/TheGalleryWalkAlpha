using Parse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TheGalleryWalk.Models;

namespace TheGalleryWalk.Controllers
{
    public class Async_SignInController : AsyncController
    {


        public ActionResult LoginNext()
        {
            Debug.WriteLine("Login Next Called from Async Sign In");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LoginNext(LoginData loginData, FormCollection data)
        {
            Debug.WriteLine("Username " + loginData.EmailAddress);
            Debug.WriteLine("Password " + loginData.Password);

            if (ModelState.IsValid)
            { 
                // other fields can be set just like with ParseObject
                try
                {   
                    Debug.WriteLine("Before User Create");
                    await ParseUser.LogInAsync(loginData.EmailAddress, loginData.Password);
                    Debug.WriteLine("Post Sign up user");
                    return View("CompletedLogin");

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

        public async Task<ActionResult> OwnedGalleries()
        {

            if(ParseUser.CurrentUser != null)
            {
                var user = ParseUser.CurrentUser;
                Debug.WriteLine("Parse user Name: " + user.Get<String>("Username"));
                Debug.WriteLine("Parse user: " + user.Get<String>("Email"));
            }
         

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