using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Syngenta.Web.FoodStandards.Spanish
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.LowercaseUrls = true;

            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Account", action = "Index" },
                namespaces: new[] { "Syngenta.Web.FoodStandards.Spanish.Controllers" }
            );

            routes.MapRoute(
                name: "Register",
                url: "register",
                defaults: new { controller = "Account", action = "Register" },
                namespaces: new[] { "Syngenta.Web.FoodStandards.Spanish.Controllers" }
            );

            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { controller = "Account", action = "Login" },
                namespaces: new[] { "Syngenta.Web.FoodStandards.Spanish.Controllers" }
            );
            routes.MapRoute(
                name: "Foreword",
                url: "foreword",
                defaults: new { controller = "Account", action = "Foreword" },
                namespaces: new[] { "Syngenta.Web.FoodStandards.Spanish.Controllers" }
            ); 

            routes.MapRoute(
                name: "ForgottenPassword",
                url: "forgottenpassword",
                defaults: new { controller = "Account", action = "ForgottenPassword" },
                namespaces: new[] { "Syngenta.Web.FoodStandards.Spanish.Controllers" }
            );

            routes.MapRoute(
                name: "LogOut",
                url: "logout",
                defaults: new { controller = "Account", action = "LogOut" },
                namespaces: new[] { "Syngenta.Web.FoodStandards.Spanish.Controllers" }
            );
            
            routes.MapRoute(
                name: "ResetPassword",
                url: "resetpassword/{token}",
                defaults: new { controller = "Account", action = "ResetPassword" },
                namespaces: new[] { "Syngenta.Web.FoodStandards.Spanish.Controllers" }
            );

            routes.MapRoute(
                name: "RegistrationConfirmation",
                url: "registrationconfirmation/{token}",
                defaults: new { controller = "Account", action = "RegistrationConfirmation" },
                namespaces: new[] { "Syngenta.Web.FoodStandards.Spanish.Controllers" }
            );

            routes.MapRoute(
                name: "Assembly",
                url: "assembly",
                defaults: new { controller = "Book", action = "assembly" },
                namespaces: new[] { "Syngenta.Web.FoodStandards.Spanish.Controllers" }
            );

            routes.MapRoute(
                name: "Preview",
                url: "preview",
                defaults: new { controller = "Book", action = "preview" },
                namespaces: new[] { "Syngenta.Web.FoodStandards.Spanish.Controllers" }
            );

            routes.MapRoute(
                name: "Download",
                url: "download",
                defaults: new { controller = "Book", action = "download" },
                namespaces: new[] { "Syngenta.Web.FoodStandards.Spanish.Controllers" }
            );

            routes.MapRoute(
                name: "DownloadPdf",
                url: "downloadpdf",
                defaults: new { controller = "Book", action = "downloadpdf" },
                namespaces: new[] { "Syngenta.Web.FoodStandards.Spanish.Controllers" }
            );

            routes.MapRoute(
                name: "SaveCompleteBook",
                url: "savecompletebook",
                defaults: new { controller = "Book", action = "savecompletebook" },
                namespaces: new[] { "Syngenta.Web.FoodStandards.Spanish.Controllers" }
            );

            routes.MapRoute(
                name: "Error404",
                url: "error404",
                defaults: new { controller = "errors", action = "Error404" },
                namespaces: new[] { "Syngenta.Web.FoodStandards.Spanish.Controllers" }
            );

            routes.MapRoute(
                name: "Error500",
                url: "error500",
                defaults: new { controller = "errors", action = "Error500" },
                namespaces: new[] { "Syngenta.Web.FoodStandards.Spanish.Controllers" }
            );

            routes.MapRoute(
                name: "ViewPage",
                url: "{url}",
                defaults: new { controller = "book", action = "viewpage" },
                namespaces: new[] { "Syngenta.Web.FoodStandards.Spanish.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Syngenta.Web.FoodStandards.Spanish.Controllers" }
            );

        }
    }
}