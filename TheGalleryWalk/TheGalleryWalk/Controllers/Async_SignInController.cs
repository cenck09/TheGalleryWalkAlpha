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
          
            if (ModelState.IsValid)
            { 
                try
                {   
                    await ParseUser.LogInAsync(loginData.EmailAddress, loginData.Password);

                    var user = ParseUser.CurrentUser;

                    IList<string> galleryIds; // = user.Get<IList<string>>("Galleries");
                    try
                    {
                        galleryIds = user.Get<IList<string>>("Galleries");
                    }
                    catch
                    {
                        galleryIds = new List<string>();
                        user["Galleries"] = galleryIds;
                        await user.SaveAsync();
                    }

                    IEnumerable<ParseObject> GalleryEntities;
                    var galleryQuery = ParseObject.GetQuery("Gallery");
                    GalleryOwnerEntity G_Owner = new GalleryOwnerEntity();
                    G_Owner.GalleryAdd = new GalleryEntity();

                    if (galleryIds.Count > 0)
                    {
                        galleryQuery = galleryQuery.WhereContainedIn("objectId", galleryIds);

                        GalleryEntities = await galleryQuery.FindAsync();

                        G_Owner.GalleryEntities = GalleryEntities;

                        return View("~/Views/OwnedGalleries/OwnedGalleries.cshtml", "_LayoutLoggedIn", G_Owner);
                    }

                    return View("~/Views/OwnedGalleries/OwnedGalleries.cshtml", "_LayoutLoggedIn", G_Owner);

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