﻿using System;
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
              
                Debug.WriteLine("The Model is valid!");

                var user = new ParseUser()
                {
                    Username = registerData.EmailAddress,
                    Password = registerData.Password,
                    Email = registerData.EmailAddress
                };

                Debug.WriteLine("Creating user: "+user.Username);
                Debug.WriteLine("On Server: " + ParseClient.CurrentConfiguration.Server);
                Debug.WriteLine("On AppId: " + ParseClient.CurrentConfiguration.ApplicationId);
               
                // other fields can be set just like with ParseObject
                try
                {
                    Debug.WriteLine("Before User Create");
                    await user.SignUpAsync();
                    Debug.WriteLine("Post Sign up user");
                    return View("CompletedSignUp");

                }
                catch ( Exception ex)
                {
                    Debug.WriteLine("There was an error "+ ex);
                }
            
               }// end if ModelState.IsValid

            Debug.WriteLine("ReachedModel NotValid");

            return View();
        }
    }
}