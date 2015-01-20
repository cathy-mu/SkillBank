using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SkillBankWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            
            //Message Detail
            routes.MapRoute(
                "Chat", // Route name
                "chat/{id}", // URL with parameters
                new { controller = "Message", action = "Chat", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Profile", // Route name
                "profile/{id}", // URL with parameters
                new { controller = "Profile", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "AboutUs", // Route name
                "aboutus", // URL with parameters
                new { controller = "About", action = "AboutUs" }
            );

            routes.MapRoute(
                "QAndA", // Route name
                "qanda", // URL with parameters
                new { controller = "About", action = "QAndA" }
            );

            routes.MapRoute(
                "Terms", // Route name
                "terms", // URL with parameters
                new { controller = "About", action = "Terms" }
            );

            routes.MapRoute(
                "Recruitment", // Route name
                "recruitment", // URL with parameters
                new { controller = "About", action = "Recruitment" }
            );

            //routes.MapRoute(
            // name: "API",
            // url: "API/{controller}/{action}"
            // );

            routes.MapRoute(
                "Intro", // Route name
                "intro", // URL with parameters
                new { controller = "Home", action = "Intro" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            
        }
    }
}