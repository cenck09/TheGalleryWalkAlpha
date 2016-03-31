﻿using System;
using System.Web.Mvc;
using TheGalleryWalk.Models;
using Parse;
using System.Threading.Tasks;

namespace TheGalleryWalk.Controllers
{
    public class HomeController : BaseValidatorController
    {

        public ActionResult Index()
        {

            if (userIsGalleryOwner())
            {
                return View("../Home/Index", "_LayoutLoggedIn");
            }
            else if(userIsArtist())
            {
                return View("../Home/Index", "_LayoutArtistLoggedIn");
            }
            else
            {
                return View("../Home/Index", "_Layout");
            }
        }

        public ActionResult GalleryOwnerPortalView()
        {
            return View("~/Views/GalleryOwnerPortal/GalleryOwnerPortalView.cshtml");
        }

        public ActionResult ArtistPortalView()
        {
            return View("~/Views/ArtistPortal/ArtistPortalView.cshtml");
        }

        public ActionResult ArtsyPortalView()
        {
            return View("~/Views/ArtsyPortal/ArtsyPortalView.cshtml");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            if (userIsGalleryOwner())
            {
                return View("../Home/Index", "_LayoutLoggedIn");
            }
            else if(userIsArtist())
            {
                return View("../Home/Index", "_LayoutArtistLoggedIn");
            }
            else
            {
                return View("../Home/About", "_Layout");
            }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";


            if (userIsGalleryOwner())
            {
                return View("../Home/Index", "_LayoutLoggedIn");
            }
            else if (userIsArtist())
            {
                return View("../Home/Index", "_LayoutArtistLoggedIn");
            }
            else
            {
                return View("../Home/Contact", "_Layout");
            }
        }
    }
}