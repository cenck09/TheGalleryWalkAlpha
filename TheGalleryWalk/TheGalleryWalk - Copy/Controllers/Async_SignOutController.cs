using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Parse;

namespace TheGalleryWalk.Controllers
{
    public class Async_SignOutController : BaseValidatorController
    {
        // GET: Async_SignOut
       public ActionResult SignOut()
        {
            logOutUser();
            return View("../Home/Index", "_Layout");
        }
    }
}