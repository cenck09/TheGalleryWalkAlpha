using Parse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using TheGalleryWalk.Models;

namespace TheGalleryWalk
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


           try {

                ParseObject.RegisterSubclass<GeneralParseUserData>();
                ParseObject.RegisterSubclass<GeneralParseUser>();
                ParseObject.RegisterSubclass<GalleryParseClass>();
                ParseObject.RegisterSubclass<ArtworkParseClass>();
                ParseObject.RegisterSubclass<ArtistParseClass>();

                ParseClient.Initialize(new ParseClient.Configuration {
                    ApplicationId = "gallerywalktest",
                    Server = "http://104.131.127.70:1337/parse/",
                });
               Debug.WriteLine("Initalized Parse Client");

            }catch(Exception ex)
            {
                Debug.WriteLine("ERROR FOR PARSE CLIENT INIT"+ex);
            }
        }
    }
}
