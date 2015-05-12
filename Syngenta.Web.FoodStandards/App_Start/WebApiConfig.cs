﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Syngenta.Web.FoodStandards
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApiController",
                routeTemplate: "api/{controller}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}