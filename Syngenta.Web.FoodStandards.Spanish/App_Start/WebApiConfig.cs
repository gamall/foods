using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Syngenta.Web.FoodStandards.Spanish
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApiSpanish",
                routeTemplate: "api/{controller}"
            );
        }
    }
}
