using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using Syngenta.Web.FoodStandards.Spanish.Filters;
using Syngenta.Web.FoodStandards.Spanish.Areas.Admin.Models;
using Syngenta.Web.FoodStandards.Spanish.Utils;
using Syngenta;
using Syngenta.Core;
using Syngenta.Core.Contracts.Data;
using Syngenta.Core.Libraries;
using System.Configuration;

namespace Syngenta.Web.FoodStandards.Spanish.Areas.Admin.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [RequireHttps]
    public class AdminController : Controller
    {
        private const int itemsPerPage = 10;

        private ISyngentaUow Uow;

        public AdminController(ISyngentaUow uow)
        {
            Uow = uow;
        }

        //
        // GET: /Admin/Login

        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Roles.IsUserInRole(WebSecurity.CurrentUserName, "Admin") && !IsExpired())
            {
                return Redirect("dashboard");
            }
            else
            {                
                return View();
            }
        }

        //
        // POST: /Admin/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection form)
        {
            System.Threading.Thread.Sleep(100);

            if (ModelState.IsValid
                && form["Email"].IsEmailAddress()
                && form["Email"].Contains("@" + ConfigurationManager.AppSettings["login_register_email_domain"])
                && Roles.IsUserInRole(form["Email"], "Admin")
                && WebSecurity.Login(form["Email"], form["Password"], persistCookie: false)
                && WebSecurity.GetPasswordFailuresSinceLastSuccess(form["Email"]) < 5
                && WebSecurity.GetLastPasswordFailureDate(form["Email"]).AddMinutes(30) < DateTime.Now
                )
            {
                Response.Cookies[0].Expires = DateTime.Now.AddHours(3);
                // populate PreviousLoginDate and LastLoginDate 
                var entityUser = Uow.Users.GetByEmail(form["Email"]);

                entityUser.PreviousLoginDate = entityUser.LastLoginDate;
                entityUser.LastLoginDate = DateTime.Now;

                Uow.Users.Update(entityUser);
                Uow.Commit();

                return Redirect("dashboard");
            }

            WebSecurity.Logout();

            // keep a log of failed login attempts, including the email address entered, IP address used, and datestamp;
            var failed = new FailedLoginAttempt()
            {
                EmailAddress = form["email"],
                IpAddress = HttpContext.Request.UserHostAddress,
                Datestamp = DateTime.Now,
                IsAdmin = Roles.IsUserInRole(form["Email"], "Admin")
            };

            Uow.FailedLoginAttempts.Add(failed);
            Uow.Commit();

            ViewBag.Message = "Error";
            return View();
        }

        //
        // GET: /Admin/Dashboard

        [SyngentaAuthorization(Role = "Admin")]
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Dashboard(int? page, string name, string mail)
        {
            if (IsExpired())
            {
                WebSecurity.Logout();

                return Redirect("login");
            }
            else
            {
                if (String.IsNullOrEmpty(name)) name = "";
                if (String.IsNullOrEmpty(mail)) mail = "";

                ViewBag.Name = name;
                ViewBag.Mail = mail;

                if (!page.HasValue || page.Value == 0) page = 1;
                ViewBag.CurrentPage = page;

                if (!String.IsNullOrEmpty(name) || !String.IsNullOrEmpty(mail))
                {
                    User feeds = new User();
                    feeds.Forename = name;
                    feeds.Surname = name;
                    feeds.Email = mail;

                    PagedList<User> paged = GetPagedUsers(Uow.Users.AdminSearch(feeds).AsEnumerable(), page.Value, itemsPerPage);

                    AdminUsersPageViewModel aupvm = new AdminUsersPageViewModel(paged);

                    return View("dashboard", aupvm);
                }
                else
                {
                    return View("dashboard");
                }
            }
        }

        //
        // POST: /Admin/Dashboard

        [HttpPost]
        [SyngentaAuthorization(Role = "Admin")]
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Dashboard(int? page, FormCollection form)
        {

            if (IsExpired())
            {
                WebSecurity.Logout();

                return Redirect("login");
            }
            else
            {

                ViewBag.Name = form["Name"];
                ViewBag.Mail = form["Mail"];

                if (!page.HasValue || page.Value == 0) page = 1;
                ViewBag.CurrentPage = page;

                User feeds = new User();
                feeds.Forename = form["Name"].ToLower();
                feeds.Surname = form["Name"].ToLower();
                feeds.Email = form["Mail"].ToLower();

                PagedList<User> paged = GetPagedUsers(Uow.Users.AdminSearch(feeds).AsEnumerable(), page.Value, itemsPerPage);

                AdminUsersPageViewModel aupvm = new AdminUsersPageViewModel(paged);

                return View("dashboard", aupvm);
            }

        }

        //
        // POST: /Admin/EditUser

        [HttpPost]
        [SyngentaAuthorization(Role = "Admin")]
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult EditUser(IEnumerable<User> users, FormCollection form)
        {
            if (IsExpired())
            {
                WebSecurity.Logout();

                return Redirect("login");
            }
            else
            {
                foreach (var user in users)
                {
                    var entityUser = Uow.Users.GetByEmail(user.Email);

                    if (entityUser != null)
                    {
                        entityUser.IsDisabled = user.IsDisabled;
                        Uow.Users.Update(entityUser);
                        Uow.Commit();

                        if (entityUser.IsDisabled)
                        {
                            // add UserModificationLog
                            var log = new UserModificationLog()
                            {
                                AdminUserId = WebSecurity.CurrentUserId,
                                AdminIPAddress = HttpContext.Request.UserHostAddress,
                                Datestamp = DateTime.Now,
                                UserId = entityUser.Id
                            };
                            Uow.UserModificationLogs.Add(log);
                            Uow.Commit();
                        }
                    }
                }

                ViewBag.Name = form["Name"];
                ViewBag.Mail = form["Mail"];

                int intPage = int.Parse(form["CurrentPage"]);
                ViewBag.CurrentPage = intPage;

                User feeds = new User();
                feeds.Forename = form["Name"].ToLower();
                feeds.Surname = form["Name"].ToLower();
                feeds.Email = form["Mail"].ToLower();

                PagedList<User> paged = GetPagedUsers(Uow.Users.AdminSearch(feeds).AsEnumerable(), intPage, itemsPerPage);

                AdminUsersPageViewModel aupvm = new AdminUsersPageViewModel(paged);

                return View("dashboard", aupvm);
            }
        }

        #region Helpers

        private PagedList<User> GetPagedUsers(IEnumerable<User> all, int currentPage, int itemsPerPage)
        {
            return new PagedList<User>(all.OrderBy(o => o.Forename + o.Surname)
                   .Skip((currentPage - 1) * itemsPerPage)
                   .Take(itemsPerPage).ToList(), all.Count(), currentPage, itemsPerPage);
        }

        private bool IsExpired()
        {
            var user = Uow.Users.GetById(WebSecurity.CurrentUserId);

            return (user.LastLoginDate.Value.AddHours(3) < DateTime.Now) ? true : false;
        }

        #endregion

    }
}