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
                var user = new GalleryOwnerParseUser()
                {
                    Username = registerData.EmailAddress,
                    Password = registerData.Password,
                    Email = registerData.EmailAddress
                };

                user["Name"] = registerData.Name;
                user["PhoneNumber"] = registerData.PhoneNumber;
                user["Enabled"] = 0;

                try
                {
                    await user.SignUpAsync();
                    return View("CompletedSignUp");
                }
                catch ( Exception ex)
                {
                    Debug.WriteLine("There was an error "+ ex);
                    return View("ErrorSignUp");
                }
            
               }// end if ModelState.IsValid
            return View();
        }
    }
}