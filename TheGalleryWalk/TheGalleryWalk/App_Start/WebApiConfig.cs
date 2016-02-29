using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;


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



            ParseClient.Initialize(new ParseClient.Configuration {
                ApplicationId = "UJAycM2x57UYodKONBQlhXu2Kcdk6jOfZv0Q2t7x",
                Server = "http://162.243.202.76:1337/parse"
            });

        }
    }
}
