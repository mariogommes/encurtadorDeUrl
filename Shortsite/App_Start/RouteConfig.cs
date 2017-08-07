using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Shortsite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Routes in MVC are compared in the order you add them
            routes.MapRoute(
                name: "Ranking",
                url: "Ranking",
                defaults: new { controller ="Ranking", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Create",
                url: "Create-short-url",
                defaults: new { controller = "Home", action = "Create", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Redirect",
                url: "{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
