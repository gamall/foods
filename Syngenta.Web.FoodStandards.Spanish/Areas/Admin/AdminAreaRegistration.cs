using System.Web.Mvc;

namespace Syngenta.Web.FoodStandards.Spanish.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "AdminDashboard",
               "admin/dashboard/{name}/{mail}/{page}",
               new { controller = "Admin", action = "Dashboard" }
            );

            context.MapRoute(
               "AdminLogin",
               "admin/login",
               new { controller = "Admin", action = "Login" }
            );

            context.MapRoute(
               "Admin",
               "admin/{action}/{id}",
               new { controller = "Admin", id = UrlParameter.Optional }
            );

        }
    }
}
