﻿using Parse;
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

                ParseObject.RegisterSubclass<GalleryOwnerParseUser>();
                ParseObject.RegisterSubclass<GalleryParseObject>();

                ParseClient.Initialize(new ParseClient.Configuration {
                    ApplicationId = "gallerywalk",
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
