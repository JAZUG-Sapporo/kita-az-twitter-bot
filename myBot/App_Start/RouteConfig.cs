﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace myBot
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Message",
                url: "Bot/{id}/Message/{action}/{messageID}",
                defaults: new { controller = "Message", action = "Index", id = "", messageID = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "BotMaster",
                url: "Bot/{id}/Master/{action}/{masterID}",
                defaults: new { controller = "BotMaster", action = "Index", id = "", masterID = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ExtensionScript",
                url: "Bot/{id}/ExtensionScript/{action}/{scriptID}",
                defaults: new { controller = "ExtensionScript", action = "Index", id = "", scriptID = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
