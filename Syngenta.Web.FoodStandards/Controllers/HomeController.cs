using Syngenta.Core;
using Syngenta.Core.Contracts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Syngenta.Web.FoodStandards.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult ViewPage(string url)
        {
            return View(url);
        }        

    }
}
