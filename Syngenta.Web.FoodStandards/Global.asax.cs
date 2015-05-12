using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Syngenta.Web.Configurations;
using Syngenta.Web.FoodStandards.Controllers;
using WebMatrix.WebData;

namespace Syngenta.Web.FoodStandards
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            IocConfig.RegisterIoc(GlobalConfiguration.Configuration);
            AuthConfig.RegisterAuth();
        }

        protected void Application_BeginRequest()
        {
            if (FormsAuthentication.RequireSSL && !Request.IsSecureConnection)
            {
                Response.Redirect(Request.Url.AbsoluteUri.Replace("http://", "https://"));
            }
        }

        protected void Application_EndRequest()
        {
            //if (Context.Response.StatusCode == 404)
            //{
            //    Response.Clear();

            //    var rd = new RouteData();
            //    //rd.DataTokens["area"] = "AreaName"; // In case controller is in another area
            //    rd.Values["controller"] = "Errors";
            //    rd.Values["action"] = "NotFound";

            //    IController c = new ErrorsController();
            //    c.Execute(new RequestContext(new HttpContextWrapper(Context), rd));
            //}
            //if (Context.Response.StatusCode == 500)
            //{
            //    Response.Clear();

            //    var rd = new RouteData();
            //    //rd.DataTokens["area"] = "AreaName"; // In case controller is in another area
            //    rd.Values["controller"] = "Errors";
            //    rd.Values["action"] = "Error500";

            //    IController c = new ErrorsController();
            //    c.Execute(new RequestContext(new HttpContextWrapper(Context), rd));
            //}
        }
    }
}