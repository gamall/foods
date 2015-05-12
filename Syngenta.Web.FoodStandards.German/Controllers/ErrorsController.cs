using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Syngenta.Web.FoodStandards.German.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult Error404()
        {
            Response.Status = "404 Not found";
            Response.StatusCode = 404;

            return View();
        }

        public ActionResult Error500()
        {
            Response.Status = "500 Server errror";
            Response.StatusCode = 500;
            return View();
        }
    }
}