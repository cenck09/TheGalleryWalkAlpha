using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Parse;

namespace TheGalleryWalk.Controllers
{
    public class Async_SignOutController : AsyncController
    {
        // GET: Async_SignOut
       public async Task<ActionResult> SignOut()
        {
            var user = ParseUser.CurrentUser;
            if(user == null)
            {
                return View("../Home/Index","_Layout");
            }
            else
            {
                await ParseUser.LogOutAsync();

            }
            return View("../Home/Index", "_Layout");
        }
    }
}