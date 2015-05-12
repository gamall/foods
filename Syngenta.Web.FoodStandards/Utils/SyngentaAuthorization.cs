using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Syngenta.Web.FoodStandards.Utils
{
    public class SyngentaAuthorization : AuthorizeAttribute
    {
        public string Url { get; set; }

        public string Role { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)        
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.HttpContext.Response.Redirect(String.IsNullOrEmpty(Url) ? "/login" : Url);
            }
            else
            {
                string[] currentRoles = System.Web.Security.Roles.GetRolesForUser(filterContext.HttpContext.User.Identity.Name);
                if (!currentRoles.Contains(Role))
                {
                    filterContext.HttpContext.Response.Redirect(String.IsNullOrEmpty(Url) ? "/login" : Url);
                }
            }
            base.OnAuthorization(filterContext);
        }
    }
}