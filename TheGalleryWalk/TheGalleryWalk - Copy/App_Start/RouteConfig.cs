using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TheGalleryWalk
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            /*
            routes.MapRoute(
               name: "Gallery",
               url: "Gallery/GalleryView/{id}",
               defaults: new { controller = "Gallery", action = "GalleryView", id = UrlParameter.Optional }
           );*/

            routes.MapRoute(
                name: "OwnedGalleries",
                url: "OwnedGalleries/OwnedGalleries/{id}",
                defaults: new { controller = "OwnedGalleries", action = "OwnedGalleries", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
