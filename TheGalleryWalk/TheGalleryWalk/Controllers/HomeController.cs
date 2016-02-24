using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheGalleryWalk.Models;

namespace TheGalleryWalk.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

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
                Console.WriteLine("The Model is valid!");
                ViewBag.Message = loginData.EmailAddress;
                return View("LoginNext");
            }
            else
            {
                return View("Login");
            }
        }

        public ActionResult Signup()
        {


            return View();
        }

        [HttpPost]
        public ActionResult Signup(string donateForm, RegisterData registerData,FormCollection variables)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine("The Model is valid!");
                return View("CompletedSignUp");
            }// end if ModelState.IsValid


            return View();
        }
        public ActionResult LoginNext()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginNext(string donateForm, LoginData loginData, FormCollection variables)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine("The Model is valid!");
                ViewBag.Message = loginData.EmailAddress;
                return View("CompletedLogin");
            }
            else
            {
                return View("LoginNext");
            }
        }

    }
}