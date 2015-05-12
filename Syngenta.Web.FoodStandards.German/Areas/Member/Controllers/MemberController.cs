using Syngenta.Core.Contracts.Data;
using Syngenta.Core;
using Syngenta.Web.FoodStandards.German.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using Syngenta.Web.FoodStandards.German.Controllers;

namespace Syngenta.Web.FoodStandards.Areas.Member.Controllers
{
    public class MemberController : Controller
    {
        //
        // GET: /Member/Member/

        protected ISyngentaUow Uow { get; set; }

        public MemberController(ISyngentaUow uow)
        {
            Uow = uow;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User u)
        {
            User user = new User();
            user.Forename = u.Forename;
            user.Surname = u.Surname;
            user.Email = u.Email;

            Uow.Commit();

            return View();

        }

    }
}
